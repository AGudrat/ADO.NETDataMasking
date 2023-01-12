
using System.Data;
using System.Data.SqlClient;
using System.Text;
using static System.ConsoleColor;
using Alba.CsConsoleFormat;

namespace ADO.NETtask
{
    public class Program
    {
        public static void Main()
        {
            string connectionString = "Server=WINDOWS-PU636AO;Database=DynamicDataMaskingDb;Trusted_Connection=True;";
            string tableNamesQueryString =
               "Execute as User = 'Test'\r\nSELECT FirstName,LastName,CardNumber FROM dbo.Users\r\nREVERT";
            var tableNames = new List<object>();
            Console.OutputEncoding = Encoding.UTF8;

            var bufferConsole = new ConsoleBuffer(width: 100);
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand tableNameCommand = new SqlCommand(tableNamesQueryString, connection))
                {
                    tableNameCommand.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(tableNameCommand))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            var space = 0;
                            bufferConsole.DrawHorizontalLine(x: 0, y: 0, width: dt.Columns.Count * 27, color: Green);
                            bufferConsole.DrawHorizontalLine(x: 0, y: (dt.Rows.Count + 1) * 2, dt.Columns.Count * 27, color: Green);
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                bufferConsole.DrawString(x: i * 30, y: 1, color: Red, text: dt.Columns[i].ToString());
                                bufferConsole.DrawVerticalLine(x: dt.Columns.Count * dt.Columns[i].ToString().Length + space, y: 1, height: (dt.Rows.Count + 1) *2, color: Green);
                                space += dt.Columns.Count * dt.Columns[i].ToString().Length;
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    bufferConsole.DrawString(x: i * 30, y: (j + 1) * 2, color: Yellow, text: dt.Rows[j][i].ToString());
                                }
                            }

                        }
                    }
                }

                new ConsoleRenderTarget().Render(bufferConsole);
            }

        }
    }

}
