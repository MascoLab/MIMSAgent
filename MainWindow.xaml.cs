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
using System.Windows.Threading;
using System.Data;
using System.IO;

namespace MIMSAgent
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
            conn = new SqlConnection("server=" + txb_ip.Text + "," + txb_port.Text + ";uid=" + txb_id.Text + ";pwd=" + txb_password.Text + ";database=" + txb_database.Text + ";");
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

            List<string> DriveList = new List<string>(); 
            foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())  //hd_usage잘안나옴
            {
                // Fixed
                //Console.WriteLine(drive.DriveType);
                //Console.WriteLine(drive.RootDirectory);

                if (drive.IsReady)
                {
                    //Console.WriteLine(drive.Name);
                    //Console.WriteLine(drive.TotalSize);
                    //Console.WriteLine(drive.AvailableFreeSpace);
                    //Console.WriteLine();

                    var driveUsage = drive.TotalSize - drive.AvailableFreeSpace;
                    var driveUsageMB = driveUsage / 1048576;
                    var driveTotMB = drive.TotalSize / 1048576;

                    var txbDrive = "Drive Type : " + drive.DriveType + "\n" + "Drive RootDirectory : " + drive.RootDirectory + "\n" + "Drive Name : " + drive.Name + "\n"
                                    + "Drive TotalSize : " + driveTotMB + "\n" + "Drive Usage : " + driveUsageMB + "\n";
                    DriveList.Add(txbDrive + "\n");
                }
            }
            txb_drive.Text = DriveListTemp(DriveList);

            Thread myThread = new Thread(checkSystem);
            myThread.Start();
        }


        public string ProcessListTemp(List<string> ProcessList)
        {
            return string.Join("", ProcessList.ToArray());
        }

        public string DriveListTemp(List<string> DriveList)
        {
            return string.Join("", DriveList.ToArray());
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

            PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total"); //_Total => Process.GetCurrentProcess().ProcessName는 특정 프로세스 정보를 얻을 수 있음
            PerformanceCounter ram = new PerformanceCounter("Memory", "Available MBytes");
            // PerformanceCounter ram1 = new PerformanceCounter("Memory", "Committed Bytes");
            // PerformanceCounter ram2 = new PerformanceCounter("Memory", "Commit Limit");
            //PerformanceCounter ram3 = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            // PerformanceCounter ram4 = new PerformanceCounter("Memory", "Pool Paged Bytes");
            // PerformanceCounter ram5 = new PerformanceCounter("Memory", "Pool Nonpaged Bytes");
            // PerformanceCounter ram6 = new PerformanceCounter("Memory", "Cache Bytes");
            //PerformanceCounter ram_max = new PerformanceCounter("Memory", "Total MBytes");
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
                sqlComm.Parameters.Clear();
                sqlComm.CommandText = "SELECT ISNULL(MAX(SERVER_SEQ), 0) + 1 FROM AGENT_SERVER_RESOURCE_TEST"; // AGENT_SERVER_RESOURCE 나중에 바꿔주어야함 //UI부분도 MASCO_TEST => HMNS_AGENT로
                try
                {
                    conn.Open();

                    using (SqlDataReader sqlRs = sqlComm.ExecuteReader())
                    {
                        while (sqlRs.Read())
                        {
                            serverId = sqlRs[0].ToString();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    StringBuilder errorMessages = new StringBuilder();
                    for (int i = 0; i < ex.Errors.Count; i++)
                    {
                        errorMessages.Append("Message: " + ex.Errors[i].Message);
                    }
                    Console.WriteLine(errorMessages.ToString());
                    //데이터베이스 내에서 속성이 변경되면 발생하는 에러부분 예외처리
                    MessageBox.Show(errorMessages.ToString());
                }
                sqlComm.ExecuteNonQuery(); //test
                conn.Close();

                //Console.WriteLine("ID : " + serverId);
                //Console.WriteLine("IP : " + serverIp);

                //Debug.WriteLine("CPU : " + cpu.NextValue().ToString() + "%");
                //Debug.WriteLine("MEMORY : " + ram.NextValue().ToString() + "MB");
                //Console.WriteLine("HDD : " + hdd.NextValue().ToString());
                // Debug.WriteLine("MEMORY : " + (ram1.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram2.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram3.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram4.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram5.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY : " + (ram6.NextValue() / 1024).ToString() + "MB");
                // Debug.WriteLine("MEMORY(MAX) : " + ram_max.RawValue + "MB");
                // Debug.WriteLine("PROCESS : " + process_name + "(" + prcess_cpu.NextValue().ToString() + "%)");
                //Console.WriteLine();




                ManagementClass objMC = new ManagementClass("Win32_OperatingSystem"); //총 메모리양 구하기 위해서 사용
                ManagementObjectCollection objMOC = objMC.GetInstances();
                var TotMo = "";
                var testCPU = "";
                foreach (ManagementObject objMO in objMOC)
                {
                    TotMo = objMO["TotalVisibleMemorySize"].ToString();
                }


                float TotMoConvert = float.Parse(TotMo);
                var TotMoDivision = TotMoConvert / 1024;
                int cpuConvert = (int)cpu.NextValue();

                //메모리 사용량 = 전체 용량(TotalMBytes) - 사용가능 용량(Available MBytes)
                var MemoryUsage = TotMoDivision - ram.NextValue();
                var dateTime = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");

                //+"MEMORY(Available) : " + ram.NextValue().ToString() + "MB" + "\n"
                var usage = "ID : " + serverId + "\n" + "IP : " + serverIp + "\n" + "CPU : " + cpuConvert + "%" + "\n" + "MEMORY(USAGE) : " + MemoryUsage + "MB" + "\n"
                            + "MEMORY(MAX) : " + TotMoDivision + "MB" + "\n" + "DateTime : " + dateTime + "\n";
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { txb_usage.Text = usage; }));
                //다른 스레드에서 해당 개체를 사용하므로 액세스 오류발생하기 때문에 스레드간의 자원을 공유하기위해 사용

                
                //Process processInfo2 = Process.GetCurrentProcess(); //현재 사용하고 있는 프로세스. 구글에 c# GetProcess() 참고
                Process[] processInfo = Process.GetProcesses();
                List<string> ProcessList = new List<string>();
                foreach (Process p in processInfo)
                {
                    var txbProcess = "Process Name: " + p.ProcessName;
                    ProcessList.Add(txbProcess + "\n");
                }
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { txb_process.Text = ProcessListTemp(ProcessList); }));
                //MASCO_TEST => AGENT_SERVER_RESOURCE_PROCESS_TEST
                sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE_PROCESS_TEST values (@param2,@param3,@param4,@param5,@param6)";
                //sqlComm.Parameters.AddWithValue("@param1", SERVER_PROCESS_SEQ);
                sqlComm.Parameters.AddWithValue("@param2", serverId);
                sqlComm.Parameters.AddWithValue("@param3", "asdf"); 
                sqlComm.Parameters.AddWithValue("@param4", cpuConvert);
                sqlComm.Parameters.AddWithValue("@param5", MemoryUsage);
                sqlComm.Parameters.AddWithValue("@param6", dateTime);
                conn.Open();
                sqlComm.ExecuteNonQuery();
                conn.Close();



                //MASCO_TEST => AGENT_SERVER_RESOURCE_TEST
                sqlComm.Parameters.Clear();
                sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE_TEST values (@param1,@param2,@param3,@param4,@param5,@param6)";
                sqlComm.Parameters.AddWithValue("@param1", serverId);  
                sqlComm.Parameters.AddWithValue("@param2", serverIp); 
                sqlComm.Parameters.AddWithValue("@param3", cpuConvert); 
                sqlComm.Parameters.AddWithValue("@param4", MemoryUsage); 
                sqlComm.Parameters.AddWithValue("@param5", TotMoDivision); 
                sqlComm.Parameters.AddWithValue("@param6", dateTime); 
                conn.Open();
                sqlComm.ExecuteNonQuery();
                conn.Close();



                //MASCO_TEST => AGENT_SERVER_RESOURCE_HDD_TEST
                foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        var driveUsage = drive.TotalSize - drive.AvailableFreeSpace;
                        var driveUsageMB = driveUsage / 1048576;
                        var driveTotMB = drive.TotalSize / 1048576;

                        sqlComm.Parameters.Clear();
                        sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE_HDD_TEST values (@param2,@param3,@param4,@param5,@param6)";
                        //sqlComm.Parameters.AddWithValue("@param1", SERVER_HDD_SEQ); 
                        sqlComm.Parameters.AddWithValue("@param2", serverId);
                        sqlComm.Parameters.AddWithValue("@param3", drive.Name); 
                        sqlComm.Parameters.AddWithValue("@param4", driveUsageMB);
                        sqlComm.Parameters.AddWithValue("@param5", driveTotMB); 
                        sqlComm.Parameters.AddWithValue("@param6", dateTime);
                        conn.Open();
                        sqlComm.ExecuteNonQuery();
                        conn.Close();
                    }
                }




                Thread.Sleep(10000);
            } while (true);
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

        private void Txb_database_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
