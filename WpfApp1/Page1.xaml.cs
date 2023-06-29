using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        public bool isPlay = false;

        public Page1()
        {
            InitializeComponent();
            // 初始化进度条和value
            timelineSlider.Minimum = 0;
            timelineSlider.Maximum = 0; 
            timelineSlider.Value = 0;
            playOrPause.Focus();
        }

        private void MediaPlay()
        {
            myMediaElement.Play();
            isPlay = true;
            playOrPause.Content = "暂停";
        }

        private void MediaPause()
        {
            myMediaElement.Pause();
            isPlay = false;
            playOrPause.Content = "播放";
        }

        void media_Switch(object sender, RoutedEventArgs e)
        {
            // 打开新的对话框
            OpenFileDialog ofd = new OpenFileDialog();
            // 如果选择了新的文件
            if (ofd.ShowDialog() == true)
            {
                // 将媒体控件的source替换
                string path = ofd.FileName;
                myMediaElement.Source = new Uri(path);
                // 实测不play后pause的话，media_Opened不会执行，暂不清楚什么原因，学艺不精，总之这样可以起作用，就先用着吧
                //myMediaElement.Play();
                //myMediaElement.Pause();
                MediaPause();
                myMediaElement.Position = new TimeSpan(0, 0, 0, 0, 1);
                //isPlay = false;
                //playOrPause.Content = "播放";
                //MessageBox.Show(myMediaElement.Position.ToString());
            }
        }

        DispatcherTimer timer = null;

        void Media_Opened(object sender, RoutedEventArgs e)
        {
            // 初始化，将timelineSlider的最大值设为TotalMilliseconds;
            timelineSlider.Minimum = 0;
            timelineSlider.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            //媒体文件打开成功
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
            //MessageBox.Show(timelineSlider.Maximum.ToString());
            //Console.WriteLine(timelineSlider.Maximum.ToString());

        }
        void timer_tick(object sender, EventArgs e)
        {
            if (!_isIgnoreCursorTime)
            {
                //MessageBox.Show(myMediaElement.Position.TotalMilliseconds.ToString());
                timelineSlider.Value = (int)myMediaElement.Position.TotalMilliseconds;
            }
        }

        void media_PlayOrPause(object sender, RoutedEventArgs e)
        {
            if(isPlay)
            {
                // 暂停视频
                MediaPause();
                //myMediaElement.Pause();
                //isPlay = false;
                //playOrPause.Content = "播放";
            }
            else
            {
                // 播放视频
                MediaPlay();
                //myMediaElement.Play();
                //isPlay = true;
                //playOrPause.Content = "暂停";
            }
        }

        // 获取媒体的进度条
        void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            //int timeSliderValue = (int)timelineSlider.Value;
            //TimeSpan ts = new TimeSpan( 0, 0, 0, 0, timeSliderValue);
            //myMediaElement.Position = ts;
            STMP();
        }

        //是否忽略播放进度更新
        bool _isIgnoreCursorTime = false;
        public void STMP()
        {
            if (!_isIgnoreCursorTime)
            {
                int timeSliderValue = (int)timelineSlider.Value;
                TimeSpan ts = new TimeSpan(0, 0, 0, 0, timeSliderValue);
                myMediaElement.Position = ts;
            }
        }


        private void sd_cursorTime_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {    //忽略播放进度更新，让进度条拖动不受影响。
            _isIgnoreCursorTime = true;
            //MessageBox.Show("1");
        }
        private void sd_cursorTime_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            //拖动中实时定位。根据需求，也可以不使用。
            STMP();
        }
        private void sd_cursorTime_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            //还原标识，然进度条重新变化。
            _isIgnoreCursorTime = false;
            //播放器定位
            STMP();
            //MessageBox.Show("2");
            //if(!myMediaElement.CanPause)
            //{
            //    myMediaElement.Play();
            //    myMediaElement.Stop();
            //}
        }

        // 鼠标定位
        private void timelineSlider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
            //根据鼠标点击位置的x值计算定位的Value值，下列代码是轨道宽等于slider控件宽的算法，如果情况不同情自行调整。
            var value = (e.GetPosition(timelineSlider).X / timelineSlider.ActualWidth) * (timelineSlider.Maximum - timelineSlider.Minimum);
            //播放器定位
            int temp = (int)value;
            //MessageBox.Show(temp.ToString());
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, temp);
            myMediaElement.Position = ts;
            timelineSlider.Value = temp;
        }

        private void myMediaElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Console.WriteLine("click");
            //Console.WriteLine(myMediaElement.Clock.IsPaused.ToString());
            if (isPlay)
            {
                //Console.WriteLine("可以暂停");
                //myMediaElement.Pause();
                //isPlay = !isPlay;
                MediaPause();
            }
            else
            {
                //Console.WriteLine("可以播放");
                //myMediaElement.Play();
                //isPlay = !isPlay;
                MediaPlay();
            }
        }

        //public int skipMilliseconds = 100;
        

        // 自定义命令：快进
        private void CustomCommand_FastForward(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Right!");
            MainWindow? temp1 = Window.GetWindow(this) as MainWindow;
            myMediaElement.Position += TimeSpan.FromMilliseconds(temp1.SkipMilliseconds);
        }

        // 自定义命令：快退
        private void CustomCommand_FastRewind(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaElement.Position -= TimeSpan.FromMilliseconds(100);
        }

        // 自定义命令：播放/暂停
        private void CustomCommand_PlayOrPause(object sender, ExecutedRoutedEventArgs e)
        {
            if (isPlay)
            {
                // 暂停视频
                MediaPause();
                //Console.WriteLine("Right!");
            }
            else
            {
                // 播放视频
                MediaPlay();
            }
        }

        private void OpenSettingWindow(object sender, RoutedEventArgs e)
        {
            //if(Window.GetWindow(this) == null)
            //{
            //    Console.WriteLine(Window.GetWindow(this));
            //}
            //Console.WriteLine(Window.GetWindow(this));
            //setting.Owner = Window.GetWindow(this);
            //setting.Owner = Window.GetWindow(timelineSlider);

            //Window.GetWindow(setting).Owner = Window.GetWindow(this);

            //Window.GetWindow(this).Owner = setting;

            Setting setting = new Setting();
            setting.Show();
            //this.Owner = setting;
            setting.Owner = Window.GetWindow(this);


            //MainWindow? mainWindow = Window.GetWindow(this) as MainWindow;
            //mainWindow?.OpenSettingWindow();
        }
    }
}
