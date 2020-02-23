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
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.ML;
using Emgu.Util;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using Emgu.CV.UI;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        VideoCapture videoCap;
        string videofile;
        VideoInfo videoInfo;
        //List<ImageInfo> imageInfoList = new List<ImageInfo>();
        ObservableCollection<ImageInfo> imageInfoList = new ObservableCollection<ImageInfo>(); 
        //dynamic GetImages;
        //当前文件夹所在路径
        public string currentPath = "";
        //当前显示的图片
        public Image<Bgr, byte> currentImage ;
        public MainWindow()
        {
            InitializeComponent();
            imageInfoGrid.MouseLeftButtonDown += DataGrid_MouseDown1;
            //ImageInfo i1 = new ImageInfo();
            //i1.Index = 1;
            //i1.Name = "00:03:12:125";
            //i1.Content = "湖北武汉：长提洪水";

            //ImageInfo i2 = new ImageInfo();
            //i2.Index = 2;
            //i2.Name = "00:03:12:125";
            //i2.Content = "湖北武汉：长提洪水";

            //ImageInfo i3 = new ImageInfo();
            //i3.Index = 3;
            //i3.Name = "00:03:12:125";
            //i3.Content = "湖北武汉：长提洪水";

            //imageInfoList.Add(i1);
            //imageInfoList.Add(i2);
            //imageInfoList.Add(i3);

            imageInfoGrid.ItemsSource = imageInfoList;
            //ImageViewer viewer = new ImageViewer();
            videoCap = new VideoCapture(@"F:\PyCharmWorkspace\创新训练项目\src\source\movies\tv.mp4");
            videoInfo = new VideoInfo(videoCap);
            videoCap.ImageGrabbed += _capture_ImageGrabbed;
            videoCap.Start();

            currentPath = "../../img/";
            //ScriptEngine pyEngine = Python.CreateEngine();//创建Python解释器对象
            //dynamic GetImages = pyEngine.ExecuteFile(@"sb.py");//读取脚本文件
            //py.getImage();//调用脚本文件中对应的函数
            //MessageBox.Show(dd);
        }
        public int i = 0;
        public int hour = 0;
        public int minute = 0;
        public int second = 0;
        public int mill = 0;
        void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            lock (new object()) {
                videoCap.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, i);
                i += videoInfo.fps;
                hour = i / videoInfo.fps / 60 / 60;
                minute = i / videoInfo.fps / 60 % 60;
                second = i / videoInfo.fps % 60;
                mill = Convert.ToInt32(i % videoInfo.fps / videoInfo.fps * 1000);
                //MessageBox.Show(hour + "_" + minute + "_" + second + "_" + mill);
                Image<Bgr, byte> img = new Image<Bgr, byte>(videoInfo.width, videoInfo.height);
                videoCap.Retrieve(img);// QueryFrame();


                ImageInfo imageInfo = new ImageInfo();
                imageInfo.Name = string.Format("{0:D2}", hour) + "_" + string.Format("{0:D2}", minute) + "_" + string.Format("{0:D2}", second) + "_" + string.Format("{0:D3}", mill) + ".bmp";
                imageInfo.Index = imageInfoList.Count + 1;
                imageInfo.Content = "正在识别...";
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                //更新DataGrid数据源
                imageInfoList.Add(imageInfo);
                });
                img.ToBitmap().Save("../../img/" + imageInfo.Name);// "../../img/"+i+".bmp");
                                                                   //cam_ibox.Image = img;// frame.ToImage<Bgr,byte>();
            }
        }
        private void DataGrid_MouseDown1(object sender, MouseButtonEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Item_GotFocus(object sender, RoutedEventArgs e)
        {
            var item = (DataGridRow)sender;
            FrameworkElement objElement = imageInfoGrid.Columns[0].GetCellContent(item);
            if (objElement != null)
            {
                currentImage = CvInvoke.Imread(currentPath+imageInfoList.ElementAt(imageInfoGrid.SelectedIndex).Name).ToImage<Bgr,byte>();
                imageBox.Image = currentImage;
                //imageBox.ImageLocation(currentPath + imageInfoList.ElementAt(1).Name);
                //MessageBox.Show(imageInfoGrid.SelectedIndex+"");
                //CheckBox objChk = (CheckBox)objElement;
                //objChk.IsChecked = !objChk.IsChecked;
            }
        }
        private void DataGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var se = imageInfoGrid.SelectedItem;
                FrameworkElement objElement = imageInfoGrid.Columns[0].GetCellContent(se);
                if (objElement != null)
                {
                   // CheckBox objChk = (CheckBox)objElement;
                   // objChk.IsChecked = !objChk.IsChecked;
                }
            }
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //打开图片
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Bitmap文件(*.bmp)|*.bmp|Jpeg文件(*.jpg)|*.jpg|所有合适文件(*.bmp/*.jpg)|*.bmp/*.jpg";
            //openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;     //对话框 关闭前还原当前目录
            if (openFileDialog.ShowDialog() == true)
            {
                videofile = openFileDialog.FileName;
                tbVideoPath.Text = videofile;

                MessageBox.Show(videofile);

                //m_Bitmap = (Bitmap)Bitmap.FromFile(name, false);  //最先打开的文件
                //this.always_Bitmap = m_Bitmap.Clone(new Rectangle(0, 0, m_Bitmap.Width, m_Bitmap.Height), PixelFormat.DontCare);
                //IntPtr ip = m_Bitmap.GetHbitmap();
                //BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                //imgLoad.Source = bitmapSource;
                //imgSplit.Source = null;
                //txtResult.Text = "";
            }
        }

    }
    public struct ImageInfo {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
    struct VideoInfo//结构体，描述视频里面的内容  
    {
        public int frameCount;
        public int width;
        public int height;
        public int currentFrame;
        public int fps;
        public VideoInfo(IntPtr capture)
        {
            frameCount = Convert.ToInt32(CvInvoke.cveVideoCaptureGet(capture, Emgu.CV.CvEnum.CapProp.FrameCount));
            width = Convert.ToInt32(CvInvoke.cveVideoCaptureGet(capture, Emgu.CV.CvEnum.CapProp.FrameWidth));
            height = Convert.ToInt32(CvInvoke.cveVideoCaptureGet(capture, Emgu.CV.CvEnum.CapProp.FrameHeight));
            currentFrame = Convert.ToInt32(CvInvoke.cveVideoCaptureGet(capture, Emgu.CV.CvEnum.CapProp.PosFrames));// Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_POS_FRAMES));
            fps = Convert.ToInt32(CvInvoke.cveVideoCaptureGet(capture,Emgu.CV.CvEnum.CapProp.Fps));
        }
    }
}
