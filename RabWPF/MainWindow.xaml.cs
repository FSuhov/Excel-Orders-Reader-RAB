using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MahApps.Metro.Controls;
using Rab_Forms;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;

namespace RabWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : MetroWindow
    {
        RabData X = new RabData();
        ObservableCollection<LineRab> orderedItems = new ObservableCollection<LineRab>();

        
        
        string pathToZeroFile;
        string pathToFolder;

        public MainWindow()
        {
            X.RegisterHandler(pop_readDate_message, pop_readFiles_message);
            InitializeComponent();
            DataContext = this;
           
        }

        /// <summary>
        /// Далее - два метода, которые реализуют вывод информационных сообщений
        /// в соответствующие окна о ходе чтения данных.
        /// Привязаны к делегатам класса RabData
        /// </summary>
        /// <param name="msg"></param>
        private void pop_readDate_message(string msg)
        {
            tbl_read_data.Text += msg;
            btn_folder.IsEnabled = true;
        }

        private void pop_readFiles_message(string msg)
        {
            tbl_read_files.Text += msg;
        }

        //сохранение пути к файлу с начальными данными
        //путь должен сохраниться в переменную и вывестись в lbl_source1
        private void btn_source_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".xlsx";
            
            dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                pathToZeroFile = dlg.FileName;
                lbl_source1.Content = pathToZeroFile;
                btn_read_source.IsEnabled = true;
            }            
        }

        //сохранение пути к папке с заказами
        //путь должен сохраниться в переменную и вывестись в lbl_folder1
        private void btn_folder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.RootFolder = Environment.SpecialFolder.MyComputer;
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                pathToFolder = dlg.SelectedPath;
                lbl_folder1.Content = pathToFolder;
                btn_read_folder.IsEnabled = true;
            }

            //if (tb_folder.Text.Length > 0)
            //{
            //    pathToFolder = tb_folder.Text;
            //    lbl_folder1.Content = pathToFolder;
            //    btn_read_folder.IsEnabled = true;
            //}
        }

        //считывание начальных данных, формирование каталога
        private void btn_read_source_Click(object sender, RoutedEventArgs e)
        {
            X.ReadData(pathToZeroFile);
        }

        //считывание файлов с заказами И!
        //формирование списка ненулевых товаров для вывода на второй вкладке
        private void btn_read_folder_Click(object sender, RoutedEventArgs e)
        {
            FilePicker picker = new FilePicker(pathToFolder);
            List<string> listOfFiles = picker.ReadDirectory();
            X.ReadOrders(listOfFiles);
            foreach(LineRab item in X.Lines)
            {
                if(item.Sum()>0)
                {
                    orderedItems.Add(item);
                }
            }
            lv_data.ItemsSource = orderedItems;            
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            X.WriteData(pathToFolder + @"\summary.xlsx");
        }
    }
}

//D:\IT Step\MY PROJECTS\RabWPF\excel\zeroRab.xlsx
//D:\IT Step\MY PROJECTS\RabWPF\excel\orders