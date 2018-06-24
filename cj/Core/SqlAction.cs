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
                string sql = "select qh,jh from fcjlk3";
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
        public static DataTable SelectToday()
        {
            try
            {
                conn.Open();
                cmd.Connection = conn;
                SQLiteHelper sh = new SQLiteHelper(cmd);
                var sql = string.Format("select qh, jh from fcjlk3  where qh like '{0}%' order by qh desc", DateTime.Now.ToString("yyyyMMdd"));
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
        public static DataTable SelectOH(string jh,string count)
        {
            try
            {
                List<string> qhList = new List<string>();
                Dictionary<string,string> dic= new Dictionary<string, string>();
                conn.Open();
                cmd.Connection = conn;
                SQLiteHelper sh = new SQLiteHelper(cmd);
                //var sql = string.Format("select qh,jh from fcjlk3 where jh={0} order by ID desc limit 0,{1}", jh,count);
                var sql = string.Format("select qh, jh from (select qh, jh from fcjlk3 order by ID desc limit 0, {0}) where jh = {1}",  count, jh);
                DataTable dt = sh.Select(sql);

                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //MessageBox.Show((int.Parse(dr["ID"].ToString()) +1).ToString());
                        //var qh1 = dr["qh"].ToString().Substring(0, 7);
                        //var qh2= (int.Parse(dr["qh"].ToString().Substring(8, 3)) + 1).ToString().PadLeft(3, '0');
                        //var qh = qh1 + qh2;
                        //qhList.Add(qh);
                        //MessageBox.Show(dr["qh"].ToString());
                        //MessageBox.Show(dr["jh"].ToString());
                    }
                    
                    foreach (var l in qhList)
                    {
                        //MessageBox.Show(l.ToString());
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
        public static DataTable SelectMaxCount(string count)
        {
            try
            {
                List<string> qhList = new List<string>();
                Dictionary<string, JhCount> dic = new Dictionary<string, JhCount>();
                conn.Open();
                cmd.Connection = conn;
                SQLiteHelper sh = new SQLiteHelper(cmd);
                var sql = string.Format("select * from fcjlk3  order by ID desc limit 0,{0}",count);
                DataTable dt = sh.Select(sql);

                if (dt.Rows.Count != 0)
                {
                    // 集合 dic 用于存放统计结果
                    
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dic.ContainsKey(dr["jh"].ToString()))
                        {
                            dic[dr["jh"].ToString()].RepeatNum += 1;
                        }
                        else
                        {
                            // 数组元素首次出现，向集合中添加一个新项
                            // 注意 ItemInfo类构造函数中，已经将重复
                            // 次数设置为 1
                            dic.Add(dr["jh"].ToString(), new JhCount(dr["jh"].ToString()));
                        }
                        
                        //MessageBox.Show((int.Parse(dr["ID"].ToString()) + 1).ToString());
                        //var qh1 = dr["qh"].ToString().Substring(0, 7);
                        //var qh2= (int.Parse(dr["qh"].ToString().Substring(8, 3)) + 1).ToString().PadLeft(3, '0');
                        //var qh = qh1 + qh2;
                        //qhList.Add(qh);
                        //MessageBox.Show(dr["qh"].ToString());
                        //MessageBox.Show(dr["jh"].ToString());
                    }
                    //清空数据
                    dt.Clear();
                    //删除列 
                    dt.Columns.Remove("ID");
                    dt.Columns.Remove("qh");
                    dt.Columns.Remove("jh");
                    //调整列顺序 ，列排序从0开始  
                    //dt.Columns["num"].SetOrdinal(1);
                    //修改列标题名称  
                    //dt.Columns["num"].ColumnName = "搜索量";
                    //dt.Columns["rate"].ColumnName = "百分比";
                    dt.Columns.Add("开奖号");
                    dt.Columns.Add("出现次数");
                    
                    foreach (JhCount info in dic.Values )
                    {
                        //MessageBox.Show(string.Format("数组元素 {0} 出现的次数为 {1}", info.Value, info.RepeatNum));
                        dt.Rows.Add(info.Value, info.RepeatNum.ToString().PadLeft(4, '0'));
                    }
                    /*
                    foreach (var l in qhList)
                    {
                        //MessageBox.Show(l.ToString());
                    }
                    */
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
        public static DataTable SelectNextID(string qh, string count)
        {
            try
            {
                if (count == "")
                {
                    count = "2500";
                }
                
                List<string> qhList = new List<string>();
                List<string> IDList = new List<string>();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                conn.Open();
                cmd.Connection = conn;
                SQLiteHelper sh = new SQLiteHelper(cmd);
                //var sql = string.Format("select qh,jh from fcjlk3 where jh={0} order by ID desc limit 0,{1}", jh,count);
                var sql = string.Format("select jh from fcjlk3 where qh = {0}", qh);
                DataTable dt = sh.Select(sql);

                sql = string.Format("select ID from (select * from fcjlk3 order by ID desc limit 0, {0}) where jh = {1}", count, dt.Rows[0]["jh"].ToString());
                dt = sh.Select(sql);
                if (dt.Rows.Count != 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        //MessageBox.Show(dr["ID"].ToString());
                        IDList.Add((int.Parse(dr["ID"].ToString()) + 1).ToString());                       
                        //var qh1 = dr["qh"].ToString().Substring(0, 7);
                        //var qh2= (int.Parse(dr["qh"].ToString().Substring(8, 3)) + 1).ToString().PadLeft(3, '0');
                        //var qh = qh1 + qh2;
                        //qhList.Add(qh);
                        //MessageBox.Show(dr["qh"].ToString());
                        //MessageBox.Show(dr["jh"].ToString());
                    }
                    dt.Clear();
                    //sh.BeginTransaction();
                    sql = "select qh,jh from fcjlk3  where ID = 0";
                    foreach (var id in IDList)
                    {
                        sql += " or ID= " + id.ToString();
                        
                        //MessageBox.Show(l.ToString());
                    }
                    dt = sh.Select(sql+ " order by qh desc");
                    //sh.Commit();
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
