using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.InteropServices;
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
using static System.Net.Mime.MediaTypeNames;

namespace suduko_solver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        int[,] tab = new int[9,9];
        int[,] tk =
      {
            {7,0,2,0,5,0,6,0,0 },
            {0,0,0,0,0,3,0,0,0 },
            {1,0,0,0,0,9,5,0,0 },
            {8,0,0,0,0,0,0,9,0 },
            {0,4,3,0,0,0,7,5,0 },
            {0,9,0,0,0,0,0,0,8 },
            {0,0,9,7,0,0,0,0,5 },
            {0,0,0,2,0,0,0,0,0 },
            {0,0,7,0,4,0,2,0,3 },
        };
        int i, j;
        
        DispatcherTimer t=new DispatcherTimer();
        
        public MainWindow()
        {
            InitializeComponent();
            hkk();
          



        }
        
        public void hkk() 
        {
            int i, j;
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    TextBox t = new TextBox();
                    t.Name = "g" + Convert.ToString(i) + Convert.ToString(j);
                    t.FontSize = 30;
                    t.TextAlignment = TextAlignment.Right;
                    t.Foreground = Brushes.Black;
                    t.BorderThickness = new Thickness(0.5);
                    t.BorderBrush = Brushes.AliceBlue;
                   
                   

                    Grid.SetRow(t, i);
                    Grid.SetColumn(t, j);
                    textboxess.Children.Add(t);
                                   
                }

            }
        
        }
       

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    TextBox t = new TextBox();
                    t = Find(textboxess,i,j);
                    t.Text = null;
                    t.IsReadOnly = false;



                }
            }
        }

        private void get_Click(object sender, RoutedEventArgs e)
        {
            for(i=0; i < 9; i++)
            {
                for(j=0; j < 9; j++)
                {
                    TextBox t = new TextBox();
                    t=Find(textboxess, i, j);



                    if (tk[i, j] != 0)
                    {
                        if (t != null)
                        {
                            t.Text = tk[i, j].ToString();
                            t.IsReadOnly = true;

                        }
                    }
                    else
                        t.Text = "";
                }
            }            

        }

        private void solvet_Click(object sender, RoutedEventArgs e)
        {
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    TextBox t = new TextBox();
                    t = Find(textboxess, i, j);
                    try
                    {
                        if (Convert.ToInt32(t.Text) != 0 && !string.IsNullOrEmpty(t.Text))
                        {
                            tab[i, j] = Convert.ToInt32(t.Text);
                        }
                        else
                        {
                            tab[i, j] = 0;
                        }
                    }catch(Exception ) { }

                }
            }
            Solve(tab);
            if (Solve(tab))
            {

                for (i = 0; i < 9; i++)
                {
                    for (j = 0; j < 9; j++)
                    {
                        TextBox t = new TextBox();
                        t = Find(textboxess, i, j);
                        t.Text = tab[i, j].ToString();
                        t.IsReadOnly = true;


                    }
                }
            }
            else MessageBox.Show("ma tt7lch ):");

        }
        public static TextBox Find(Grid grid, int row, int column)
       {
            foreach (UIElement child in grid.Children)
            {
                if(child is TextBox)
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                {
                    return (TextBox)child;
                }
                
            }
            return null;
        }


        static int size = 9;

        static bool IsInRow(int[,] tab, int row, int num)
        {
            for (int i = 0; i < size; i++)
                if (tab[row, i] == num)
                    return true;
            return false;

        }
        static bool IsInColom(int[,] tab, int colom, int num)
        {
            for (int i = 0; i < size; i++)
                if (tab[i, colom] == num)
                    return true;
            return false;

        }

        static bool IsInSquare(int[,] tab, int row, int colom, int num)
        {
            int i, j, r, c;
            r = row - row % 3;
            c = colom - colom % 3;
            for (i = r; i < r + 3; i++)
                for (j = c; j < c + 3; j++)
                    if (tab[i, j] == num)
                        return true;
            return false;




        }
        static bool IsPlaceValide(int[,] tab, int row, int colom, int num)
        {
            return !IsInColom(tab, colom, num) && !IsInRow(tab, row, num) && !IsInSquare(tab, row, colom, num);
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            int[,] tabl = new int[9, 9];
            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    TextBox t=new TextBox();
                    t=Find(textboxess, i, j);
                    try
                    {
                        tabl[i,j]=Convert.ToInt32(t.Text);

                    }catch(Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
            Solve(tk);
            if (IsEqual(tk,tabl))
                MessageBox.Show("it's Correct");
            else
                MessageBox.Show("it's not Correct");
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        static bool Solve(int[,] tab)
        {
            int i, j, nt;
            for (i = 0; i < size; i++)
            {
                for (j = 0; j < size; j++)
                {
                    if (tab[i, j] == 0)
                    {
                        for (nt = 1; nt <= size; nt++)
                        {
                            if (IsPlaceValide(tab, i, j, nt))
                            {
                                tab[i, j] = nt;

                                if (Solve(tab))
                                    return true;
                                else
                                    tab[i, j] = 0;

                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsEqual(int [,]t1,int [,]t2)
        {
            for (i = 0; i < 9; i++)
                for (j = 0; j < 9; j++)
                    if (t1[i, j] != t2[i, j])
                        return false;
            return true;




        }









    }
}
