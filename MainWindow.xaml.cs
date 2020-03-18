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
        /*
                private string serverId;
                private string serverHddId;
                private string serverProcessID;
                private string serverIp;
        */
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


        float tempCPU;

        private void checkSystem()
        {
            string serverIp = null;

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
            PerformanceCounter hdd = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");

            PerformanceCounter processCpu1 = new PerformanceCounter("Process", "% Processor Time", "kairos"); //특정 프로세스 cpu 사용량
            PerformanceCounter processCpu2 = new PerformanceCounter("Process", "% Processor Time", "DailyDetecterConsole_x64_v1_2_0_1");
            PerformanceCounter processCpu3 = new PerformanceCounter("Process", "% Processor Time", "sqlservr");

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
                ManagementClass objMC = new ManagementClass("Win32_OperatingSystem"); //총 메모리양 구하기 위해서 사용
                ManagementObjectCollection objMOC = objMC.GetInstances();
                var TotMo = "";
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

                //AGENT_SERVER_RESOURCE QUERY      //insertServerResource <- insert
                string serverId = insertServerResource(serverIp, cpuConvert, MemoryUsage, TotMoDivision);


                //+"MEMORY(Available) : " + ram.NextValue().ToString() + "MB" + "\n"
                var usage = "ID : " + serverId + "\n" + "IP : " + serverIp + "\n" + "CPU : " + cpuConvert + "%" + "\n" + "MEMORY(USAGE) : " + MemoryUsage + "MB" + "\n"
                            + "MEMORY(MAX) : " + TotMoDivision + "MB" + "\n" + "DateTime : " + dateTime + "\n";
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { txb_usage.Text = usage; }));
                //다른 스레드에서 해당 개체를 사용하므로 액세스 오류발생하기 때문에 스레드간의 자원을 공유하기위해 사용


                //specific process cpu,memory usage test
                //PerformanceCounter processCpu = new PerformanceCounter("Process", "% Processor Time", "Taskmgr"); //특정 프로세스 cpu 사용량
                //PerformanceCounter processMemory = new PerformanceCounter("Process", "Working Set - Private", "Taskmgr"); //연결된 프로세스의 실제 메모리 사용량
                ////PerformanceCounter processDisk = new PerformanceCounter("PhysicalDisk", "% Idle Time", "_Total"); 
                //var processMemoryConvert = processMemory.NextValue() / 1048576;
                //processCpu.NextValue();
                //Thread.Sleep(10000); 
                //Console.WriteLine("Process CPU : " + processCpu.NextValue() + "%"); 
                //Console.WriteLine("Process Memory : " + processMemoryConvert + "MB");
                ////Console.WriteLine("Process Disk : " + processDisk.NextValue() + "MB/s");
                //Console.WriteLine();


                //AGENT_SERVER_RESOURCE_PROCESS QUERY
                Process[] processInfo = Process.GetProcesses();
                List<string> ProcessList = new List<string>();
                foreach (Process p in processInfo)
                {
                    var processName = p.ProcessName;
                    var txbProcess = "Process Name: " + processName;
                    ProcessList.Add(txbProcess + "\n");
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate { txb_process.Text = ProcessListTemp(ProcessList); }));

                    //PerformanceCounter processCpu = new PerformanceCounter("Process", "% Processor Time", processName); //특정 프로세스 cpu 사용량
                    PerformanceCounter processMemory = new PerformanceCounter("Process", "Working Set - Private", processName); //연결된 프로세스의 실제 메모리 사용량

                    bool isKairosChecked = chb_kairos.Dispatcher.Invoke(new Func<bool>(() => (bool)chb_kairos.IsChecked));
                    bool isSqlChecked = chb_sql.Dispatcher.Invoke(new Func<bool>(() => (bool)chb_sql.IsChecked));
                    bool isdailyChecked = chb_daily.Dispatcher.Invoke(new Func<bool>(() => (bool)chb_daily.IsChecked));

                    //PerformanceCounter가 do-while 밖에 있어야 cpu의 정확한 값을 얻을 수 있어서(thread때문에 안에 있으면 항상 0만뜸) tempCPU라는 임시공간에 저장하여 쿼리문 작성함.
                    if (processName == "kairos")
                    {
                        tempCPU = processCpu1.NextValue();
                    }
                    else if (processName == "DailyDetecterConsole_x64_v1_2_0_1")
                    {
                        tempCPU = processCpu2.NextValue();
                    }
                    else if (processName == "sqlservr")
                    {
                        tempCPU = processCpu3.NextValue();
                    }


                    if (processName == "kairos" && isKairosChecked == true ||
                        processName == "DailyDetecterConsole_x64_v1_2_0_1" && isdailyChecked == true ||
                        processName == "sqlservr" && isSqlChecked == true)
                        
                    {
                        var processMemoryConvert = processMemory.NextValue() / 1048576;

                        sqlComm.Parameters.Clear();
                        /*
                        sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE_PROCESS values (" +
                            "( SELECT ISNULL(MAX(SERVER_PROCESS_SEQ), 0) + 1 FROM AGENT_SERVER_RESOURCE_PROCESS )," +
                            "@param2,@param3,@param4,@param5,GETDATE() )";
                        */
                        sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE_PROCESS " +
                                                "SELECT " +
                                                    "ISNULL( MAX( SERVER_PROCESS_SEQ ), 0 ) + 1, " +
                                                    "@param2, " +
                                                    "@param3, " +
                                                    "@param4, " +
                                                    "@param5, " +
                                                    "GETDATE() " +
                                                "FROM AGENT_SERVER_RESOURCE_PROCESS";

                        //sqlComm.Parameters.AddWithValue("@param1", serverProcessID);
                        sqlComm.Parameters.AddWithValue("@param2", serverId);
                        sqlComm.Parameters.AddWithValue("@param3", processName);
                        sqlComm.Parameters.AddWithValue("@param4", tempCPU);
                        sqlComm.Parameters.AddWithValue("@param5", processMemoryConvert);
                        //sqlComm.Parameters.AddWithValue("@param6", dateTime);
                        conn.Open();
                        sqlComm.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                //AGENT_SERVER_RESOURCE_HDD QUERY
                foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        var driveUsage = drive.TotalSize - drive.AvailableFreeSpace;
                        var driveUsageMB = driveUsage / 1048576;
                        var driveTotMB = drive.TotalSize / 1048576;

                        sqlComm.Parameters.Clear();
                        /*
                        sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE_HDD values (" +
                            "(SELECT ISNULL(MAX(SERVER_HDD_SEQ), 0) + 1 FROM AGENT_SERVER_RESOURCE_HDD)," +
                            "@param2,@param3,@param4,@param5,GETDATE())";
                        */
                        sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE_HDD " +
                                                "SELECT " +
                                                    "ISNULL( MAX( SERVER_HDD_SEQ ), 0 ) + 1, " +
                                                    "@param2, " +
                                                    "@param3, " +
                                                    "@param4, " +
                                                    "@param5, " +
                                                    "GETDATE() " +
                                                "FROM AGENT_SERVER_RESOURCE_HDD";
                        //sqlComm.Parameters.AddWithValue("@param1", serverHddId);
                        sqlComm.Parameters.AddWithValue("@param2", serverId);
                        sqlComm.Parameters.AddWithValue("@param3", drive.Name);
                        sqlComm.Parameters.AddWithValue("@param4", driveUsageMB);
                        sqlComm.Parameters.AddWithValue("@param5", driveTotMB);
                        //sqlComm.Parameters.AddWithValue("@param6", dateTime);
                        conn.Open();
                        sqlComm.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                Thread.Sleep(10000);
            } while (true);
        }


        private string insertServerResource(String serverIp, int cpuConvert, float MemoryUsage, float TotMoDivision)
        {
            string serverId = null;

            sqlComm.Parameters.Clear();

            sqlComm.CommandText = "SELECT ISNULL(MAX(SERVER_SEQ), 0) + 1 FROM AGENT_SERVER_RESOURCE";
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

            sqlComm.Parameters.Clear();
            sqlComm.CommandText = "insert into dbo.AGENT_SERVER_RESOURCE values (@param1,@param2,@param3,@param4,@param5, GETDATE() )";
            sqlComm.Parameters.AddWithValue("@param1", serverId);
            sqlComm.Parameters.AddWithValue("@param2", serverIp);
            sqlComm.Parameters.AddWithValue("@param3", cpuConvert);
            sqlComm.Parameters.AddWithValue("@param4", MemoryUsage);
            sqlComm.Parameters.AddWithValue("@param5", TotMoDivision);
            //          sqlComm.Parameters.AddWithValue("@param6", dateTime);
            conn.Open();
            sqlComm.ExecuteNonQuery();
            conn.Close();

            return serverId;
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
