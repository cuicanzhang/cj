using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace cj.Core
{
    class SqlAction
    {
        private static SQLiteConnection conn = new SQLiteConnection(config.DataSource);
        private static SQLiteCommand cmd = new SQLiteCommand();

        public static bool AddH(Dictionary<string, object> dic)
        {
            try
            {
                conn.Open();
                cmd.Connection = conn;
                SQLiteHelper sh = new SQLiteHelper(cmd);
                var sql = string.Format("select * from fcjlk3 where (qh='{0}')", dic["qh"]);
                DataTable dt = sh.Select(sql);
                if (dt.Rows.Count == 0)
                {
                    sh.Insert("fcjlk3", dic);
                    return true;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
        public static DataTable SelectH(string searchStr)
        {
            try
            {
                conn.Open();
                cmd.Connection = conn;
                SQLiteHelper sh = new SQLiteHelper(cmd);
                string sql = "select * from fcjlk3";
                DataTable dt = sh.Select(sql);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                DataTable dt = new DataTable();
                return dt;
            }
            finally
            {
                conn.Close();
            }

        }
        public static DataTable SelectOH(string jh)
        {
            try
            {
                List<string> qhList = new List<string>();
                conn.Open();
                cmd.Connection = conn;
                SQLiteHelper sh = new SQLiteHelper(cmd);
                var sql = string.Format("select * from fcjlk3 where jh={0}", jh);
                DataTable dt = sh.Select(sql);

                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var qh1 = dr["qh"].ToString().Substring(0, 7);
                        var qh2= (int.Parse(dr["qh"].ToString().Substring(8, 3)) + 1).ToString().PadLeft(3, '0');
                        var qh = qh1 + qh2;
                        qhList.Add(qh);
                        //MessageBox.Show(dr["qh"].ToString());
                        //MessageBox.Show(dr["jh"].ToString());
                    }
                    
                    foreach (var l in qhList)
                    {
                        MessageBox.Show(l.ToString());
                    }
                    return dt;
                }
                
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                DataTable dt = new DataTable();
                return dt;
            }
            finally
            {
                conn.Close();
            }

        }
    }
}
