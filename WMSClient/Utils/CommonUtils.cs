using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WMSClient.Utils
{
    /// <summary>
    /// 通用工具类（所有页面共享）
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// 泛型列表转DataTable（内箱/外箱页面都用到）
        /// </summary>
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            // 创建列（处理可空类型）
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // 填充数据（处理空值）
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(values);
            }

            return table;
        }

        /// <summary>
        /// 统一的弹窗提示（避免每个页面重复写MessageBox）
        /// </summary>
        public static void ShowMessage(string content, string title = "提示", MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            MessageBox.Show(content, title, MessageBoxButtons.OK, icon);
        }

        /// <summary>Parse server response: array = success, object with Code/Msg = error. Returns (list, errorMessage).</summary>
        public static (List<T> list, string errorMessage) SafeParseListResponse<T>(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                return (new List<T>(), null);
            try
            {
                JToken token = JToken.Parse(response);
                if (token is JArray arr)
                {
                    var list = arr.ToObject<List<T>>() ?? new List<T>();
                    return (list, null);
                }
                if (token is JObject obj)
                {
                    string msg = obj["Msg"]?.Value<string>() ?? "Server returned an error.";
                    return (null, msg);
                }
            }
            catch (JsonException ex)
            {
                return (null, "Invalid response: " + ex.Message);
            }
            return (new List<T>(), null);
        }

        /// <summary>Get the row that should be used for Edit/Delete. Prefers SelectedRows[0] so menu click does not use stale CurrentRow.</summary>
        public static DataGridViewRow GetSelectedRow(DataGridView dgv)
        {
            if (dgv == null) return null;
            if (dgv.SelectedRows.Count > 0) return dgv.SelectedRows[0];
            return dgv.CurrentRow;
        }

        /// <summary>Confirm dialog (shared for delete / important actions).</summary>
        public static bool ShowConfirm(string content, string title = "Confirm")
        {
            return MessageBox.Show(content, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// DataGridView通用粘贴逻辑（内箱/外箱页面都用到）
        /// </summary>
        public static void HandleDataGridViewPaste(
            DataGridView dgv,
            KeyEventArgs e,
            DataGridViewCellEventHandler cellValueChangedHandler // 修正：使用专属委托类型
        )
        {
            try
            {
                string clipboardText = Clipboard.GetText().Trim();
                if (string.IsNullOrEmpty(clipboardText))
                {
                    ShowMessage(GlobalConstants.MsgNoData, "提示", MessageBoxIcon.Warning);
                    e.Handled = true;
                    return;
                }

                string[] lines = clipboardText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 0)
                {
                    ShowMessage("粘贴数据格式错误", "提示", MessageBoxIcon.Warning);
                    e.Handled = true;
                    return;
                }

                if (dgv.SelectedCells.Count == 0)
                {
                    ShowMessage("请先选择要粘贴的单元格区域", "提示", MessageBoxIcon.Warning);
                    e.Handled = true;
                    return;
                }

                var selectedCells = dgv.SelectedCells.Cast<DataGridViewCell>().ToList();
                int startCol = selectedCells.Min(c => c.ColumnIndex);
                int startRow = selectedCells.Min(c => c.RowIndex);

                int rowsToPaste = lines.Length;
                string[] firstRowCells = lines[0].Split('\t');
                int colsToPaste = firstRowCells.Length;

                if (startRow + rowsToPaste > dgv.RowCount || startCol + colsToPaste > dgv.ColumnCount)
                {
                    ShowMessage("粘贴数据超出表格范围", "错误", MessageBoxIcon.Error);
                    e.Handled = true;
                    return;
                }

                // 修正：临时取消CellValueChanged事件订阅（使用正确的委托类型）
                if (cellValueChangedHandler != null)
                {
                    dgv.CellValueChanged -= cellValueChangedHandler;
                }
                dgv.SuspendLayout();

                for (int rowIdx = 0; rowIdx < rowsToPaste; rowIdx++)
                {
                    string[] cells = lines[rowIdx].Split('\t');
                    for (int colIdx = 0; colIdx < Math.Min(colsToPaste, cells.Length); colIdx++)
                    {
                        if (!dgv[startCol + colIdx, startRow + rowIdx].ReadOnly)
                        {
                            dgv[startCol + colIdx, startRow + rowIdx].Value = cells[colIdx].Trim();
                        }
                    }
                }

                // 修正：重新订阅CellValueChanged事件
                dgv.ResumeLayout();
                if (cellValueChangedHandler != null)
                {
                    dgv.CellValueChanged += cellValueChangedHandler;
                }

                e.Handled = true;
            }
            catch (Exception ex)
            {
                ShowMessage($"粘贴失败：{ex.Message}", "错误", MessageBoxIcon.Error);
                Console.WriteLine($"HandleDataGridViewPaste Error: {ex}");
                e.Handled = true;
            }
        }
    

        /// <summary>
        /// 安全更新UI（跨线程操作共用）
        /// </summary>
        public static void SafeInvoke(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}