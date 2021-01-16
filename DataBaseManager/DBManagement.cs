using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLCommand;

namespace DataBaseManager
{
    public class DBManagement
    {
        private SqlConnection conn = null;

        public DBManagement()
        {
            XMLCommand.Command cmd = new XMLCommand.Command(System.IO.Directory.GetCurrentDirectory(),"Config.xml","Config");
            string connectionString = string.Format("Server={0}; Database={1}; uid={2};pwd={3}", cmd.Read("DBAddress"), cmd.Read("DBName"), cmd.Read("DBAcount"), cmd.Read("DBPass"));
            conn = new SqlConnection(connectionString);
        }

        /// <summary>
        /// 2021-01-14<br></br>
        /// 고한열<br></br><br></br>
        /// DB오픈
        /// </summary>
        public void DBOpen()
        {
            conn.Open();
        }

        /// <summary>
        /// 2021-01-14<br></br>
        /// 고한열<br></br><br></br>
        /// DB닫기
        /// </summary>
        public void DBClose()
        {
            conn.Close();
        }

        /// <summary>
        /// 2021-01-14<br></br>
        /// 고한열<br></br><br></br>
        /// DB와 Reader 닫기
        /// </summary>
        /// <param name="sr"></param>
        private void DBClose(SqlDataReader sr)
        {
            conn.Close();
            sr.Close();
        }

        /// <summary>
        /// 2021-01-14<br></br>
        /// 고한열<br></br><br></br>
        /// DB 읽기
        /// </summary>
        /// <param name="sQuery"></param>
        /// <returns></returns>
        public DataTable SelectDB(string sQuery)
        {
            DataTable returnValue = new DataTable();

            SqlDataReader sr = null;
            SqlCommand comm = new SqlCommand(sQuery, conn);

            DBOpen();

            sr = comm.ExecuteReader();

            returnValue = sr.GetSchemaTable();

            DBClose(sr);

            return returnValue;
        }

        public void InsertDB(string sQuery)
        {
            SqlCommand comm = new SqlCommand(sQuery, conn);
            DBOpen();
            comm.ExecuteNonQuery();
            DBClose();
        }

        public void DeleteDB(string sQuery)
        {
            InsertDB(sQuery);
        }

        public void UpdateDB(string sQuery)
        {
            InsertDB(sQuery);
        }
    }
}
