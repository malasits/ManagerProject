using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Net.NetworkInformation;
using System.Windows.Threading;

namespace ManagerProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Timer
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Application started...");

            //Időzítő indítása
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Start();

            CommandText.Focus();
            CommandPalette.Visibility = Visibility.Visible;
            CommandLoader.Visibility = Visibility.Collapsed;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //Parancs bevitele és szelektálása
            if (e.Key == Key.Enter)
            {
                //Ha nem üres a beviteli mező
                if (!string.IsNullOrEmpty(CommandText.Text))
                {
                    CommandPalette.Visibility = Visibility.Collapsed;
                    CommandLoader.Visibility = Visibility.Visible;

                    CommandText.Text = CommandText.Text.ToUpper();
                    CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " <-- " + CommandText.Text);

                    // WEBOLDAL BENYITÁSOK  **************************************************
                    //GOOGLE TÉRKÉP MEGNYITÁSA
                    if (CommandText.Text.Contains("MAP"))
                    {
                        if (CommandText.Text.Contains("CITY"))
                        {
                            try
                            {
                                string[] searchText = CommandText.Text.Split();
                                Process.Start("https://www.google.com/maps/place/" + searchText[2]);
                                CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Map opened and the searched city:[- " + searchText[2] + " -]");
                            }
                            catch (Exception error)
                            {
                                CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> ERROR - " + error.Message);
                                CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> ERROR - There is an error in the command!");
                            }

                        }
                        else
                        {//Alap térkép
                            Process.Start("https://www.google.com/maps/?hl=hu");
                            CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Map opened...");
                        }
                    }

                    //Youtube megnyitása
                    else if (CommandText.Text.Contains("YOUTUBE"))
                    {
                        if (CommandText.Text.Contains("SEARCH"))
                        {
                            try
                            {
                                string[] searchText = CommandText.Text.Split();
                                Process.Start("https://www.youtube.com/results?search_query=" + searchText[2]);
                                CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Youtube opened and the searched video:[- " + searchText[2] + " -]");
                            }
                            catch (Exception error)
                            {
                                CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> ERROR - " + error.Message);
                                CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> ERROR - There is an error in the command!");
                            }
                        }
                        else
                        {
                            Process.Start("https://www.youtube.com");
                            CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Youtube opened...");
                        }
                    }

                    //NCORE MEGNYITÁSA
                    else if (CommandText.Text.Contains("NCORE"))
                    {
                        //NCORE AKTIVITÁS MEGNYITÁSA
                        if (CommandText.Text == "NCORE ACTIVITY")
                        {
                            Process.Start("https://ncore.cc/hitnrun.php");
                            CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Ncore Activity opened...");
                        }
                        else
                        {
                            Process.Start("https://ncore.cc/index.php");
                            CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Ncore opened...");
                        }
                    }

                    // KONZOL TULAJDONSÁGOK  **************************************************
                    //Konzol kiürítése
                    else if (CommandText.Text.Contains("CLEAR"))
                    {
                        CommandPalette.Items.Clear();
                        CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Console cleared...");
                    }
                    //Alkalmazás bezárása
                    else if (CommandText.Text.Contains("EXIT"))
                    {
                        this.Close();
                    }

                    //Hibás parancs!!
                    else
                    {
                        CommandPalette.Items.Add(DateTime.Now.ToString("hh:mm:ss") + " --> Unknonw command [- " + CommandText.Text + " -]");
                    }

                    CommandText.Text = "";
                    CommandText.Focus();

                    CommandPalette.Visibility = Visibility.Visible;
                    CommandLoader.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //Időzített funkciók
            using (Ping p = new Ping())
            {
                //Internet kapcsolat ellenőrzése
                try
                {
                    if(p.Send("www.google.com").Status.ToString() == "Success")
                    {
                        InternetConnectionText.Foreground = Brushes.DarkGreen;
                        InternetConnectionText.Content = "Connected!";
                    }
                    else
                    {
                        InternetConnectionText.Foreground = Brushes.DarkOrange;
                        InternetConnectionText.Content = p.Send("www.google.com").Status.ToString();
                    }
                }
                catch /*(Exception error)*/
                {
                    InternetConnectionText.Foreground = Brushes.Red;
                    InternetConnectionText.Content = "Not Connected";
                }

                //Ping lekérdezése
                try
                {
                    //NA: 104.160.131.3
                    //EUW: 104.160.141.3
                    //EUNE: 104.160.142.3
                    //OCE: 104.160.156.1
                    //LAN - 104.160.136.3
                    int ping = Convert.ToInt32(p.Send("104.160.141.3").RoundtripTime.ToString());
                    if(ping < 65)
                    {
                        InternetPingText.Foreground = Brushes.Green;
                        InternetPingText.Content = ping + "ms";
                    }
                    if(ping > 64 && ping < 90)
                    {
                        InternetPingText.Foreground = Brushes.DarkOrange;
                        InternetPingText.Content = ping + "ms";
                    }
                    if(ping > 89)
                    {
                        InternetPingText.Foreground = Brushes.Red;
                        InternetPingText.Content = ping + "ms";
                    }
                    

                }
                catch (Exception)
                {
                    InternetPingText.Foreground = Brushes.Red;
                    InternetPingText.Content = "-";
                }
            }
            
        }
    }
}
