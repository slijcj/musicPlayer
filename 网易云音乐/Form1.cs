using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace 网易云音乐
{
    public partial class Form1 : Form
    {
        double max, min, bai;
        Thread th1;//程序集线程变量

        

        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 载入窗口时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Form1_Load(object sender, EventArgs e)
        {
            th1 = new Thread(new ThreadStart(r));
            th1.IsBackground = true;
            th1.Start();//开启线程
        }

        private delegate void read_value();//实例化一个委托对象
        private void r()
        {
            read_value rv = new read_value(read);
            this.Invoke(rv);//调用委托对象
        }

        /// <summary>
        /// 读取保存的歌曲信息文件
        /// </summary>
        private void read()
        {
            System.IO.FileStream fs = new System.IO.FileStream("C:\\Users\\ckp\\source\\repos\\网易云音乐\\网易云音乐\\bin\\Debug\\temp.txt",System.IO.FileMode.Open,System.IO.FileAccess.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs,Encoding.Default);
            while (!sr.EndOfStream)
            {
                listBox1.Items.Add(sr.ReadLine());//添加到列表框中
            }
            sr.Close();
            fs.Close();
            th1.Abort();//关闭线程

        }

        /// <summary>
        /// 当鼠标按下时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            timer_jc.Enabled = false;//停止检测播放进度
            axWindowsMediaPlayer2.Ctlcontrols.pause();//暂停播放文件

        }
        /// <summary>
        /// 当鼠标放下时执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            double now_value = (trackBar1.Value * 0.1) * 0.1 * max;//还原播放器
            axWindowsMediaPlayer2.Ctlcontrols.currentPosition = now_value;//重置播放进度
            axWindowsMediaPlayer2.Ctlcontrols.play();//继续播放
            timer_jc.Enabled = true;//继续检测播放进度
        }
        /// <summary>
        /// 双击列表框执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            axWindowsMediaPlayer2.URL = listBox1.SelectedItem.ToString();
        }

        private void btn_tz_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer2.Ctlcontrols.stop();//停止播放
        }

        private void btn_bf_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer2.Ctlcontrols.play();//播放
        }

        private void btn_zt_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer2.Ctlcontrols.pause();//暂停播放
        }
        /// <summary>
        /// 关闭窗口执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //保存播放列表的歌曲
            string save = "";
            for(int i = 0; i < listBox1.Items.Count; i++)
            {
                save += listBox1.Items[i].ToString() + "\r\n";
            }
            System.IO.FileStream fs = new System.IO.FileStream("C:\\Users\\ckp\\source\\repos\\网易云音乐\\网易云音乐\\bin\\Debug\\temp.txt", System.IO.FileMode.Create,System.IO.FileAccess.Write);//实例化一个文件流
            System.IO.StreamWriter sw = new System.IO.StreamWriter(fs,Encoding.Default);
            sw.Write(save);//写到文件
            sw.Close();//关闭文件流
            fs.Close();
        }
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex == -1)
            {
                return;
            }
            listBox1.Items.Remove(listBox1.SelectedItem);
        }
        /// <summary>
        /// 清空列表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 清空列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //清空之前先提示用户是否确认清空
           if(MessageBox.Show("是否确认清空列表？") == DialogResult.OK)
            {
                listBox1.Items.Clear();//清空
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer2.settings.volume = trackBar2.Value;//设置播放器音量为为滑块大小
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 nf = new Form2();
            nf.ShowDialog();
        }

        private void btn_tj_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();//实例化一个通用对话框
            open.Filter = "音频文件(*.mp3)|*.mp3";//写一个文件过滤器
            if (open.ShowDialog() == DialogResult.OK)
            {
                //还原最大最小值
                max = 0.0;
                min = 0.0;
                bai = 0.0;
                trackBar1.Value = 0;
                timer_jc.Enabled = false;//关闭检测
                axWindowsMediaPlayer2.URL = open.FileName;//添加到播放器
                listBox1.Items.Add(open.FileName);//将音频文件添加到列表框里面
                listBox1.SelectedIndex = listBox1.Items.Count - 1;//添加的歌曲文件
                timer_jc.Enabled = true;//开始检测进度
            }

        }
        /// <summary>
        /// 检查媒体的播放进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_jc_Tick(object sender, EventArgs e)
        {
            max = axWindowsMediaPlayer2.currentMedia.duration;//获取文件长度
            min = axWindowsMediaPlayer2.Ctlcontrols.currentPosition;//获取文件的当前播放位置
            bai = min / max;//计算出百分比
            trackBar1.Value = (int)(bai * 100);//添加到滑块中
        }
    }
}
