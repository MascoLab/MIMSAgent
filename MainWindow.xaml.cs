using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using System.Threading;
// using System.Runtime.InteropServices;
// using System.Security;
using System.Management;
using System.Reflection;

using System.Net;

using System.Data.SqlClient;

namespace MascoAgent
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private SqlConnection conn = null;
        private SqlCommand sqlComm = new SqlCommand();

        private string serverId;
        private string serverIp;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            Thread myThread = new Thread(checkSystem);
            myThread.Start();
            */
            /*
            192.168.0.203
            sa
            t320@mascolab
            */
            // conn = new SqlConnection("server =" + strDBIP + "; uid =" + strDBID + "; pwd =" + strDBPW + "; database =" + strDBName);
            // Console.WriteLine( "server="+ txb_ip.Text + "," + txb_port.Text + ";uid=" + txb_id.Text + ";pwd=" + txb_password.Text + ";database=" + txb_database.Text + ";" );
            conn = new SqlConnection("server="+ txb_ip.Text + "," + txb_port.Text + ";uid=" + txb_id.Text + ";pwd=" + txb_password.Text + ";database=" + txb_database.Text + ";");
            sqlComm.Connection = conn;
            /*
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = conn;
            sqlComm.CommandText = "SELECT ISNULL(MAX(SERVER_ID), 0) + 1 FROM MCMS_SERVER_RESOURCE";

            conn.Open();

            using( SqlDataReader sqlRs = sqlComm.ExecuteReader() )
            {
                Console.WriteLine( "### SUCCESS ###" );
                while( sqlRs.Read() )
                {
                    Console.WriteLine( sqlRs[0].ToString() );
                }
            }
            conn.Close();
            */

            ManagementClass objMC = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                Console.WriteLine(objMO["TotalVisibleMemorySize"]);
            }

            foreach(System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
            {
                // Fixed
                Console.WriteLine(drive.DriveType);
                Console.WriteLine(drive.RootDirectory);

                if ( drive.IsReady )
                {
                    Console.WriteLine(drive.Name);
                    Console.WriteLine(drive.TotalSize);
                    Console.WriteLine(drive.AvailableFreeSpace);
                }
            }

            Thread myThread = new Thread(checkSystem);
            myThread.Start();
        }

        private void checkSystem()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    //Console.WriteLine( ip.ToString() );
                    serverIp = ip.ToString();
                }
            }
            
            PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter ram = new PerformanceCounter("Memory", "Available MBytes");
            // PerformanceCounter ram1 = new PerformanceCounter("Memory", "Committed Bytes");
            // PerformanceCounter ram2 = new PerformanceCounter("Memory", "Commit Limit");
            // PerformanceCounter ram3 = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            // PerformanceCounter ram4 = new PerformanceCounter("Memory", "Pool Paged Bytes");
            // PerformanceCounter ram5 = new PerformanceCounter("Memory", "Pool Nonpaged Bytes");
            // PerformanceCounter ram6 = new PerformanceCounter("Memory", "Cache Bytes");
            // PerformanceCounter ram_max = new PerformanceCounter("Memory", "Total MBytes");
            PerformanceCounter hdd = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            /*
            PerformanceCounter("Processor", "% Processor Time", "_Total");
            PerformanceCounter("Processor", "% Privileged Time", "_Total");
            PerformanceCounter("Processor", "% Interrupt Time", "_Total");
            PerformanceCounter("Processor", "% DPC Time", "_Total");
            PerformanceCounter("Memory", "Available MBytes", null);
            PerformanceCounter("Memory", "Committed Bytes", null);
            PerformanceCounter("Memory", "Commit Limit", null);
            PerformanceCounter("Memory", "% Committed Bytes In Use", null);
            PerformanceCounter("Memory", "Pool Paged Bytes", null);
            PerformanceCounter("Memory", "Pool Nonpaged Bytes", null);
            PerformanceCounter("Memory", "Cache Bytes", null);
            PerformanceCounter("Paging File", "% Usage", "_Total");
            PerformanceCounter("PhysicalDisk", "Avg. Disk Queue Length", "_Total");
            PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Read", "_Total");
            PerformanceCounter("PhysicalDisk", "Avg. Disk sec/Write", "_Total");
            PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            PerformanceCounter("Process", "Handle Count", "_Total");
            PerformanceCounter("Process", "Thread Count", "_Total");
            PerformanceCounter("System", "Context Switches/sec", null);
            PerformanceCounter("System", "System Calls/sec", null);
            PerformanceCounter("System", "Processor Queue Length", null);
            */

            /*
            PerformanceCounter ram = new PerformanceCounter("Mono Memory", "Available Physical Memory");
            PerformanceCounter ram_max = new PerformanceCounter("Mono Memory", "Total Physical Memory");
            */

            // string process_name = Process.GetCurrentProcess().ProcessName;
            // PerformanceCounter prcess_cpu = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
            /*
            try
            {
                Process[] allProc = Process.GetProcesses();
                foreach( Process processInfo in allProc )
                {
                    Console.WriteLine("Process : {0}", processInfo.ProcessName);
                    Console.WriteLine("시작시간 : {0}", processInfo.StartTime);
                    Console.WriteLine("프로세스 PID : {0}", processInfo.Id);
                    //                    Console.WriteLine("메모리 : {0}", processInfo.VirtualMemorySize);
                    Console.WriteLine();
                }
            }
            catch ( Exception e )
            {
                Console.WriteLine( e.Message );
            }
            */
            /*
            // Sql 연결정보(서버:127.0.0.1, 포트:3535, 아이디:sa, 비밀번호 : password, db : member)
            string connectionString = "server = 127.0.0.1,3535; uid = sa; pwd = password; database = member;";
            // Sql 새연결정보 생성
            SqlConnection sqlConn = new SqlConnection(connectionString);
            SqlCommand sqlComm = new SqlCommand();
            sqlComm.Connection = sqlConn;
            sqlComm.CommandText = "insert into tbl_member (id,name,addr) values (@param1,@param2,@param3)";
            //sqlComm.CommandText = "update tbl_member set addr=@param3 where id=@param1 and name=@param2";
            //sqlComm.CommandText = "delete tbl_member where id=@param1 and name=@param2 and addr=@param3";
            sqlComm.Parameters.AddWithValue("@param1", "abc");
            sqlComm.Parameters.AddWithValue("@param2", "홍길동");
            sqlComm.Parameters.AddWithValue("@param3", "서울");
            sqlConn.Open();
            sqlComm.ExecuteNonQuery();
            sqlConn.Close();
            */
            do
            {
                sqlComm.CommandText = "SELECT ISNULL(MAX(SERVER_ID), 0) + 1 FROM MCMS_SERVER_RESOURCE";
                conn.Open();
                using (SqlDataReader sqlRs = sqlComm.ExecuteReader())
                {
                    while (sqlRs.Read())
                    {
                        serverId = sqlRs[0].ToString();
                    }
                }
                conn.Close();

                Console.WriteLine("ID : " + serverId);
                Console.WriteLine("IP : " + serverIp);
                
                Debug.WriteLine("CPU : " + cpu.NextValue().ToString() + "%");
                Debug.WriteLine("MEMORY : " + ram.NextValue().ToString() + "MB");

                Console.WriteLine("HDD : " + hdd.NextValue().ToString());
                // Debug.WriteLine("MEMORY : " + (ram1.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram2.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram3.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram4.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram5.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram6.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY(MAX) : " + ram_max.RawValue + "MB");
                // Debug.WriteLine("PROCESS : " + process_name + "(" + prcess_cpu.NextValue().ToString() + "%)");
                Console.WriteLine();
                Thread.Sleep( 10000 );
            } while( true );
        }
/*
        private void getPhysicalMemory()
        {
            int maxMem = 0;

            ObjectQuery winQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(winQuery);
            ManagementObjectCollection queryCollection = searcher.Get();

            ulong memory = 0;
            foreach (ManagementObject item in queryCollection)
            {
                memory = ulong.Parse(item["TotalVisibleMemorySize"].ToString());
            }
            maxMem = (int)(memory / 1024);
        }
*/
    }
}
