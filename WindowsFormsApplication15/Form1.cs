using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Word;

namespace WindowsFormsApplication15
{

    public partial class Form1 : Form
    {
        public string pathImage = string.Empty;
        DataKursEntities5 db = new DataKursEntities5();

        public Form1()
        {
            InitializeComponent();
            //DataBaseEntities db = new DataBaseEntities();
            db.Dishes.Load();
            dataGridView1.DataSource = db.Dishes.Local.ToBindingList();

            db.Dishesmenu.Load();
            dataGridView3.DataSource = db.Dishesmenu.Local.ToBindingList();
            db.Menu.Load();
            dataGridView2.DataSource = db.Menu.Local.ToBindingList();


            db.Recipes.Load();
            dataGridView4.DataSource = db.Recipes.Local.ToBindingList();


            button13.Enabled = false;

            button3.Enabled = false;

            button8.Enabled = false;

            button10.Enabled = false;

            //comboBox3.Text = "";

            //comboBox2.Text = "";

            //comboBox1.Text = "";

            //dataGridView1.Rows[0].Cells[0].Selected = false;
            //dataGridView1.ClearSelection();


            //db.Dishesmenu.Load();
            //dataGridView3.DataSource = db.Dishesmenu.Local.ToBindingList();

            //var result = from Di in db.Dishesmenu
            //             join Menu in db.Menu on Di.idMenu equals Menu.Id
            //             select new
            //             {
            //                 idMenu = Menu.Id

            //             };
            //dataGridView3.DataSource = result.ToList();



            if (dataGridView2.RowCount > 0)
            {
                int o = (int)dataGridView2.Rows[0].Cells[0].Value;
                var result = from Di in db.Dishesmenu

                             where Di.idMenu == o
                             orderby Di.Category
                             select new
                             {
                                 Id = Di.Id,
                                 idDishes = Di.idDishes,
                                 Name = Di.Name,
                                 Ves = Di.Ves,
                                 Price = Di.Price,
                                 idMenu = o,
                                 Category = Di.Category

                             };
                dataGridView3.DataSource = result.ToList();
                textBox4.Text = dataGridView2.Rows[0].Cells[0].Value.ToString();
            }


            if (dataGridView1.RowCount > 0)
            {
                int i = (int)dataGridView1.Rows[0].Cells[0].Value;
                var result5 = from Dishes in db.Dishes
                              join Recipes in db.Recipes on Dishes.Id equals Recipes.idDishes
                              where Recipes.idDishes == i
                              select new
                              {
                                  Id = Recipes.Id,
                                  Name = Recipes.Name,
                                  Composition = Recipes.Composition,
                                  Description = Recipes.Description,
                                  Category = Recipes.Category,
                                  idDishes = i
                              };


                dataGridView5.DataSource = result5.ToList();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dataKursDataSet.Dishesmenu". При необходимости она может быть перемещена или удалена.
            this.dishesmenuTableAdapter.Fill(this.dataKursDataSet.Dishesmenu);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dataKursDataSet.Menu". При необходимости она может быть перемещена или удалена.
            this.menuTableAdapter.Fill(this.dataKursDataSet.Menu);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dataKursDataSet.Recipes". При необходимости она может быть перемещена или удалена.
            this.recipesTableAdapter.Fill(this.dataKursDataSet.Recipes);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "dataKursDataSet.Dishes". При необходимости она может быть перемещена или удалена.
            this.dishesTableAdapter.Fill(this.dataKursDataSet.Dishes);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (NameDish.Text == "" || CategoryDish.Text == "" || DescripDish.Text == "" || VesDish.Text == "" || PriceDish.Text == "" || label7.Text == "")
            { MessageBox.Show("Одно из значений имеет пустое поле", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else
            {
            
            //DataBaseEntities db = new DataBaseEntities();
            Dishes Dishname = new Dishes();
            //int id = db.Dishes.Max(x => x.Id);
            //Dishname.Id = id + 1;
            Dishname.Name = NameDish.Text;
            Dishname.Category = CategoryDish.Text;
            Dishname.Description = DescripDish.Text;
            Dishname.Ves = Convert.ToInt32(VesDish.Text);
            Dishname.Price = Convert.ToDecimal(PriceDish.Text);
            byte[] imgBrands = null;
            FileStream fsBrand = new FileStream(pathImage, FileMode.Open, FileAccess.Read);
            BinaryReader brBrand = new BinaryReader(fsBrand);
            imgBrands = brBrand.ReadBytes((int)fsBrand.Length);
            Dishname.ImagesDish = imgBrands;
            db.Dishes.Add(Dishname);
            db.SaveChanges();

            NameDish.Text = "";
            CategoryDish.Text = "";
            DescripDish.Text = "";
            VesDish.Text = "";
            PriceDish.Text = "";
            label7.Text = "";
            MessageBox.Show("Новый объект добавлен", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

            db.Dishes.Load();
            dataGridView1.DataSource = db.Dishes.Local.ToBindingList();
            this.dishesTableAdapter.Fill(this.dataKursDataSet.Dishes);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bmp = new Bitmap(dataGridView1.Size.Width + 1000, dataGridView1.Size.Height + 1000);
            dataGridView1.DrawToBitmap(bmp, dataGridView1.Bounds);
            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
            //printDocument1.Print();
        }
       

        private void button1_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Images (*.jpg; *.jpeg; *.gif; *.bmp; *.ico; *.png) | *.jpg; *.jpeg; *.gif; *.bmp; *.ico; *.png";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathImage = openFileDialog1.FileName.ToString();

            }
            label7.Text = openFileDialog1.FileName.ToString();


       
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (NameDish.Text == "" || CategoryDish.Text == "" || DescripDish.Text == "" || VesDish.Text == "" || PriceDish.Text == "")
            {
                MessageBox.Show("Одно или более поле имеет пустое значение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int n = dataGridView1.SelectedRows[0].Index;
                    dataGridView1.Rows[n].Cells[1].Value = NameDish.Text;
                    dataGridView1.Rows[n].Cells[2].Value = CategoryDish.Text;
                    dataGridView1.Rows[n].Cells[3].Value = DescripDish.Text;
                    dataGridView1.Rows[n].Cells[4].Value = VesDish.Text;
                    dataGridView1.Rows[n].Cells[5].Value = PriceDish.Text;



                    if(label7.Text == "")
                    {  }
                    else { 
                    byte[] imgBrands = null;
                    FileStream fsBrand = new FileStream(pathImage, FileMode.Open, FileAccess.Read);
                    BinaryReader brBrand = new BinaryReader(fsBrand);
                    imgBrands = brBrand.ReadBytes((int)fsBrand.Length);
                    dataGridView1.Rows[n].Cells[6].Value = imgBrands;
                    }
                    db.SaveChanges();
                    AddButton.Enabled = true;
                    button3.Enabled = false;

                    NameDish.Text = "";
                    CategoryDish.Text = "";
                    DescripDish.Text = "";
                    VesDish.Text = "";
                    PriceDish.Text = "";
                    label7.Text = "";

                    MessageBox.Show("Блюдо отредактировано", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Выберите строку для редактирования.", "Ошибка.");
                }
            }

           
        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView1[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Dishes player = db.Dishes.Find(id);
                db.Dishes.Remove(player);
                db.SaveChanges();

                MessageBox.Show("Объект удален");
            }
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            int u = (int)dataGridView1.SelectedCells[0].Value;
            var result = from Dishes in db.Dishes
                         join Recipes in db.Recipes on Dishes.Id equals Recipes.idDishes
                         where Recipes.idDishes == u
                         select new
                         {
                             Id = Recipes.Id,
                             Name = Recipes.Name,
                             Composition = Recipes.Composition,
                             Description = Recipes.Description,
                             Category = Recipes.Category,
                             idDishes = u
                         };


            dataGridView5.DataSource = result.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            { MessageBox.Show("Одно или более поле имеет пустое значение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else { 
                Menu Menus = new Menu();

                Menus.Name = textBox5.Text;
                Menus.Date = dateTimePicker1.Value;

                db.Menu.Add(Menus);
                db.SaveChanges();

                textBox5.Text = "";
                MessageBox.Show("Меню добавлено", "Меню", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

    

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            //int n = dataGridView2.SelectedRows[0].Index;
            int u = (int)dataGridView2.SelectedCells[0].Value;
            //int u = (int)dataGridView2.Rows[0].Cells[0].Value;

            var result = from Di in db.Dishesmenu
                         //join Dish in db.Dishes on Di.idDishes equals Dish.Id
                         join Menu in db.Menu on Di.idMenu equals Menu.Id
                         
                         where Di.idMenu == u
                        orderby Di.Category
                         select new
                         {

                             Id = Di.Id,
                             idDishes = Di.idDishes,
                             Name = Di.Name,
                             Ves = Di.Ves,
                             Price = Di.Price,
                             idMenu = u,
                             Category = Di.Category

                         };


            dataGridView3.DataSource = result.ToList();
            textBox4.Text = Convert.ToString(dataGridView2.SelectedCells[0].Value);
            //dataGridView3.DataSource = db.Dishesmenu.Local.ToBindingList();

            //textBox5.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            //dateTimePicker1.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
            


        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if(comboBox1.Text == "")
            { MessageBox.Show("Одно или более поле имеет пустое значение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else { 
            Dishesmenu MenuDish = new Dishesmenu();


            MenuDish.Name = comboBox1.Text;
            MenuDish.Ves = Convert.ToInt32(textBox2.Text);
            MenuDish.Price = Convert.ToDecimal(textBox3.Text);
                //MenuDish.idDishes = Convert.ToInt32(textBox1.Text);
            MenuDish.idDishes = Convert.ToInt32(comboBox1.SelectedValue);
            MenuDish.Category = CategoryDishesmenu.Text;
            MenuDish.idMenu = Convert.ToInt32(textBox4.Text);

            db.Dishesmenu.Add(MenuDish);
            db.SaveChanges();

            //int n = dataGridView2.SelectedRows[0].Index;
            int u = (int)dataGridView2.SelectedCells[0].Value;
            //int u = (int)dataGridView2.Rows[0].Cells[0].Value;

            var result = from Menu in db.Menu
                         join Di in db.Dishesmenu on Menu.Id equals Di.idMenu
                         where Di.idMenu == u
                         orderby Di.Category
                         select new
                         {
                             Id = Di.Id,
                             idDishes = Di.idDishes,
                             Name = Di.Name,
                             Ves = Di.Ves,
                             Price = Di.Price,
                             idMenu = u,
                             Category = Di.Category
                         };


            dataGridView3.DataSource = result.ToList();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "" || textBox7.Text == "" || textBox8.Text == "" || CategoryRec.Text == "")
            { MessageBox.Show("Одно или более поле имеет пустое значение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            else {
      
                        

                int valueDish = (int)comboBox3.SelectedValue;

                int countRecept = db.Recipes.Count(x => x.idDishes == valueDish);

                if (countRecept == 0)
                {
                    Recipes Rec = new Recipes();

                    Rec.Name = comboBox3.Text;
                    Rec.Composition = textBox7.Text;
                    Rec.Description = textBox8.Text;
                    Rec.Category = CategoryRec.Text;
                    Rec.idDishes = Convert.ToInt32(comboBox3.SelectedValue); /*Convert.ToInt32(textBox6.Text);*/

                    db.Recipes.Add(Rec);
                    db.SaveChanges();

                    comboBox3.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    CategoryRec.Text = "";
                    MessageBox.Show("Рецепт добавлен", "Рецепт", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else { MessageBox.Show("Рецепт для данного блюда уже добавлен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error ); }
                    
                }


            }


        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Formoprogram fp = new Formoprogram();
            fp.Show();
            

        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int u = (int)dataGridView2.SelectedCells[0].Value;
            if (dataGridView2.SelectedRows.Count > 0)
            {
                //int index = dataGridView2.SelectedRows[0].Index;
                //int id = 0;

                int ind = dataGridView2.SelectedCells[0].RowIndex;
                dataGridView2.Rows.RemoveAt(ind);

                var customer = db.Dishesmenu.Where(c => c.idMenu == u).ToList();

                db.Dishesmenu.RemoveRange(customer);
                db.SaveChanges();


                


                var resultDel = from Menu in db.Menu
                                join Di in db.Dishesmenu on Menu.Id equals Di.idMenu
                                //join Dish in db.Dishes on Di.idDishes equals Dish.Id
                                where Di.idMenu == u
                                select new
                                {

                                    Id = Di.Id,
                                    idDishes = Di.idDishes,
                                    Name = Di.Name,
                                    Ves = Di.Ves,
                                    Price = Di.Price,
                                    idMenu = u

                                };
                dataGridView3.DataSource = resultDel.ToList();

                MessageBox.Show("Объект удален");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            { MessageBox.Show("Одно или более поле имеет пустое значение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); } else {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int n = dataGridView2.SelectedRows[0].Index;
                dataGridView2.Rows[n].Cells[1].Value = textBox5.Text;
                dataGridView2.Rows[n].Cells[2].Value = dateTimePicker1.Text;

                button5.Enabled = true;
                button8.Enabled = false;
                    MessageBox.Show("Меню отредактировано", "Меню", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    textBox5.Text = "";
                }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка.");
            }
            }

            db.SaveChanges();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                //int n = dataGridView2.SelectedRows[0].Index;
                //dataGridView3.Rows[n].Cells[1].Value = FindRecBox.Text;

                int index = dataGridView3.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView3[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Dishesmenu Dis = db.Dishesmenu.Find(id);

                Dis.Name = comboBox1.Text;
                Dis.Ves = Convert.ToInt32(textBox2.Text);
                Dis.Price = Convert.ToDecimal(textBox3.Text);
                Dis.Category = CategoryDishesmenu.Text;

                db.SaveChanges();
                dataGridView3.Refresh(); // обновляем грид
                db.SaveChanges();
               
                Button6.Enabled = true;
                button10.Enabled = false;
                textBox2.ReadOnly = true;
                textBox3.ReadOnly = true;

                int u = (int)dataGridView2.SelectedCells[0].Value;
                //int u = (int)dataGridView2.Rows[0].Cells[0].Value;

                var result = from Di in db.Dishesmenu
                                 //join Dish in db.Dishes on Di.idDishes equals Dish.Id
                             join Menu in db.Menu on Di.idMenu equals Menu.Id

                             where Di.idMenu == u
                             orderby Di.Category
                             select new
                             {

                                 Id = Di.Id,
                                 idDishes = Di.idDishes,
                                 Name = Di.Name,
                                 Ves = Di.Ves,
                                 Price = Di.Price,
                                 idMenu = u,
                                 Category = Di.Category

                             };


                dataGridView3.DataSource = result.ToList();

                MessageBox.Show("Объект обновлен");
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка.");
            }


            db.SaveChanges();
        }

        private void dataGridView3_MouseClick(object sender, MouseEventArgs e)
        {
            //textBox1.Text = dataGridView3.SelectedRows[0].Cells[1].Value.ToString();
            //comboBox1.Text = dataGridView3.SelectedRows[0].Cells[2].Value.ToString();
            //textBox2.Text = dataGridView3.SelectedRows[0].Cells[3].Value.ToString();
            //textBox3.Text = dataGridView3.SelectedRows[0].Cells[4].Value.ToString();
            //textBox4.Text = dataGridView3.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                int index = dataGridView3.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView3[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Dishesmenu menuDel = db.Dishesmenu.Find(id);
                db.Dishesmenu.Remove(menuDel);
                db.SaveChanges();
                int u = (int)dataGridView2.SelectedCells[0].Value;
                //int u = (int)dataGridView2.Rows[0].Cells[0].Value;

                var result = from Menu in db.Menu
                             join Di in db.Dishesmenu on Menu.Id equals Di.idMenu
                             where Di.idMenu == u
                             select new
                             {
                                 Id = Di.Id,
                                 idDishes = Di.idDishes,
                                 Name = Di.Name,
                                 Ves = Di.Ves,
                                 Price = Di.Price,
                                 idMenu = u
                             };


                dataGridView3.DataSource = result.ToList();
                MessageBox.Show("Объект удален");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGridView4.SelectedRows.Count > 0)
            {
                int index = dataGridView4.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView4[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Recipes menuDel = db.Recipes.Find(id);
                db.Recipes.Remove(menuDel);
                db.SaveChanges();

                MessageBox.Show("Объект удален");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text == "" || textBox7.Text == "" || textBox8.Text == "" || CategoryRec.Text == "")
            {
                MessageBox.Show("Одно из значений имеет пустое поле", "Неудача");
            }
            else
            {
                if (dataGridView4.SelectedRows.Count > 0)
                {
                    int n = dataGridView4.SelectedRows[0].Index;
                    dataGridView4.Rows[n].Cells[1].Value = comboBox3.Text;
                    dataGridView4.Rows[n].Cells[2].Value = textBox7.Text;
                    dataGridView4.Rows[n].Cells[3].Value = textBox8.Text;
                    dataGridView4.Rows[n].Cells[4].Value = CategoryRec.Text;
                    //dataGridView4.Rows[n].Cells[5].Value = textBox6.Text;
                    button7.Enabled = true;
                    button13.Enabled = false;


                    comboBox3.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    CategoryRec.Text = "";
                    
                }
                else
                {
                    MessageBox.Show("Выберите строку для редактирования.", "Ошибка.");
                }
            }

            db.SaveChanges();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int u = (int)dataGridView1.SelectedCells[0].Value;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить блюдо '" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + "' ?", "Подтверждение удаления записи", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    int ind = dataGridView1.SelectedCells[0].RowIndex;
                    dataGridView1.Rows.RemoveAt(ind);

                    var customer = db.Recipes.Where(c => c.idDishes == u).ToList();

                    db.Recipes.RemoveRange(customer);
                    db.SaveChanges();

                    var result = from Dishes in db.Dishes
                                 join Recipes in db.Recipes on Dishes.Id equals Recipes.idDishes
                                 where Recipes.idDishes == u
                                 select new
                                 {
                                     Id = Recipes.Id,
                                     Name = Recipes.Name,
                                     Composition = Recipes.Composition,
                                     Description = Recipes.Description,
                                     Category = Recipes.Category,
                                     idDishes = u
                                 };


                    dataGridView5.DataSource = result.ToList();

                }

             }

            }

        private void редактироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NameDish.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            CategoryDish.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            DescripDish.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            VesDish.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            PriceDish.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

            AddButton.Enabled = false;
            button3.Enabled = true;
        }

        private void удалитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
              if (dataGridView4.SelectedRows.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить рецепт для блюда '" + dataGridView4.SelectedRows[0].Cells[1].Value.ToString() + "' ?", "Подтверждение удаления записи", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (res == DialogResult.Yes)
                {
                    int index = dataGridView4.SelectedRows[0].Index;
                    int id = 0;
                    bool converted = Int32.TryParse(dataGridView4[0, index].Value.ToString(), out id);
                    if (converted == false)
                        return;

                    Recipes menuDel = db.Recipes.Find(id);
                    db.Recipes.Remove(menuDel);
                    db.SaveChanges();
                }
            }
        }

        private void редактироватьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            comboBox3.Text = dataGridView4.SelectedRows[0].Cells[1].Value.ToString();
            textBox7.Text = dataGridView4.SelectedRows[0].Cells[2].Value.ToString();
            textBox8.Text = dataGridView4.SelectedRows[0].Cells[3].Value.ToString();
            CategoryRec.Text = dataGridView4.SelectedRows[0].Cells[4].Value.ToString();
            //textBox6.Text = dataGridView4.SelectedRows[0].Cells[5].Value.ToString();
            button7.Enabled = false;
            button13.Enabled = true;
        }

        private void PriceDish_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void dataGridView4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Название: " + dataGridView5.SelectedRows[0].Cells[1].Value.ToString()
               + "\n\nСостав: " + dataGridView5.SelectedRows[0].Cells[2].Value.ToString()
               + "\n\nОписание: " + dataGridView5.SelectedRows[0].Cells[3].Value.ToString()
               + "\n\nКатегория: " + dataGridView5.SelectedRows[0].Cells[4].Value.ToString(), "Информация");
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            Dish Di = new Dish();
            Di.Show();
            Di.label1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            Di.label2.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            Di.label3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            Di.label4.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            Di.label5.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
          
            int n = dataGridView1.SelectedRows[0].Index;
            byte[] imagecell = (byte[])dataGridView1.CurrentRow.Cells[6].Value;
            byte[] imageData = (byte[])dataGridView1.Rows[n].Cells[6].Value;
            Image newImage;

            using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
            {
                ms.Write(imageData, 0, imageData.Length);

                newImage = Image.FromStream(ms, true);
            }

            Di.pictureBox1.Image = newImage;

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void редактироватьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            textBox5.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            dateTimePicker1.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();

            button5.Enabled = false;
            button8.Enabled = true;
        }

        private void удалитьToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Вы действительно хотите удалить меню '" + dataGridView2.SelectedRows[0].Cells[1].Value.ToString() + "' ?", "Подтверждение удаления записи", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (res == DialogResult.Yes)
            {
            int u = (int)dataGridView2.SelectedCells[0].Value;
                if (dataGridView2.SelectedRows.Count > 0)
                {

                    int ind = dataGridView2.SelectedCells[0].RowIndex;
                    dataGridView2.Rows.RemoveAt(ind);

                    var customer = db.Dishesmenu.Where(c => c.idMenu == u).ToList();

                    db.Dishesmenu.RemoveRange(customer);
                    db.SaveChanges();





                    var resultDel = from Menu in db.Menu
                                    join Di in db.Dishesmenu on Menu.Id equals Di.idMenu
                                    //join Dish in db.Dishes on Di.idDishes equals Dish.Id
                                    where Di.idMenu == u
                                    select new
                                    {

                                        Id = Di.Id,
                                        idDishes = Di.idDishes,
                                        Name = Di.Name,
                                        Ves = Di.Ves,
                                        Price = Di.Price,
                                        idMenu = u

                                    };
                    dataGridView3.DataSource = resultDel.ToList();

                    
                }
            }
        }

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Form2 Men = new Form2();
            Men.Show();
            Men.label1.Text = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
            //Men.label2.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();

            //Men.Label.Text = "\n";
            ////Men.label6.Text = "\n";
            //Men.label7.Text = "\n";

            Men.Label.Text = "Название\n\n";
            Men.label6.Text = "Вес\n\n";
            Men.label7.Text = "Цена\n\n";

            foreach (DataGridViewRow row in dataGridView3.Rows)
            {
                if (row.Cells[1].Value != null)
                    Men.Label.Text += row.Cells[2].Value.ToString() + "\n\n";
                Men.label6.Text += row.Cells[3].Value.ToString() + "\n\n";
                Men.label7.Text += row.Cells[4].Value.ToString() + "\n\n";
            }

        }

        private void редактироватьToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //FindRecBox.Text = dataGridView3.SelectedRows[0].Cells[1].Value.ToString();
            comboBox1.Text = dataGridView3.SelectedRows[0].Cells[2].Value.ToString();
            textBox2.Text = dataGridView3.SelectedRows[0].Cells[3].Value.ToString();
            textBox3.Text = dataGridView3.SelectedRows[0].Cells[4].Value.ToString();
            textBox4.Text = dataGridView3.SelectedRows[0].Cells[5].Value.ToString();

            CategoryDishesmenu.Text = dataGridView3.SelectedRows[0].Cells[6].Value.ToString();

            Button6.Enabled = false;
            button10.Enabled = true;

            textBox2.ReadOnly = false;
            textBox3.ReadOnly = false;
        }

        private void удалитьToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                int index = dataGridView3.SelectedRows[0].Index;
                int id = 0;
                bool converted = Int32.TryParse(dataGridView3[0, index].Value.ToString(), out id);
                if (converted == false)
                    return;

                Dishesmenu menuDel = db.Dishesmenu.Find(id);
                db.Dishesmenu.Remove(menuDel);
                db.SaveChanges();
                int u = (int)dataGridView2.SelectedCells[0].Value;
                //int u = (int)dataGridView2.Rows[0].Cells[0].Value;

                var result = from Menu in db.Menu
                             join Di in db.Dishesmenu on Menu.Id equals Di.idMenu
                             where Di.idMenu == u
                             select new
                             {
                                 Id = Di.Id,
                                 idDishes = Di.idDishes,
                                 Name = Di.Name,
                                 Ves = Di.Ves,
                                 Price = Di.Price,
                                 idMenu = u
                             };


                dataGridView3.DataSource = result.ToList();
                MessageBox.Show("Объект удален", "Блюда в меню", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            var templateFileName = Path.Combine(Environment.CurrentDirectory, "меню.doc");

            if (!File.Exists(templateFileName))
            {
                throw new FileNotFoundException("File not found.", templateFileName);
            }
            Dishesmenu Du = new Dishesmenu();
            using (var application = new NetOffice.WordApi.Application { Visible = true })
            {
                using (var document = application.Documents.Add(templateFileName))
                {

                    List<HelpCategory> cats = new List<HelpCategory>();

                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        HelpCategory temp = new HelpCategory();
                        temp.ves = (int)dataGridView3.Rows[i].Cells[3].Value;
                        temp.name = (string)dataGridView3.Rows[i].Cells[2].Value;
                        temp.price = (decimal)dataGridView3.Rows[i].Cells[4].Value;
                        temp.category = (string)dataGridView3.Rows[i].Cells[6].Value;

                        cats.Add(temp);
                    }


                    var coordinatesTable = document.Bookmarks["Coordinates"].Range.Tables[1];


                    //

                    List<string> uniqueCategory = cats.Select(x => x.category).Distinct().ToList();

                   

                    foreach (string strCat in uniqueCategory)
                    {
                        var tempWord = coordinatesTable.Rows.Add();

                        tempWord.Cells[2].Range.Bold = 3;
                        tempWord.Cells[2].Range.Text = strCat;


                        foreach (HelpCategory cat in cats)
                        {
                            if (cat.category == strCat)
                            {
                                
                                var tempRow = coordinatesTable.Rows.Add();
                                tempRow.Cells[2].Range.Bold = 0;
                                tempRow.Cells[1].Range.Text = cat.ves.ToString();
                                tempRow.Cells[2].Range.Text = cat.name;
                                tempRow.Cells[3].Range.Text = cat.price.ToString("# руб");
                            }
                        }  
                    }

                    coordinatesTable.Rows[2].Delete();

                    var MenuTable = document.Bookmarks["data"].Range;
                    int n = dataGridView2.SelectedRows[0].Index;
                    MenuTable.Text = dataGridView2.Rows[n].Cells[2].Value.ToString();
                    //var MenuTable = document.Bookmarks["Menu"].Range.Text;

                    ////var coordinateRow = Menu.Add();

                }
                application.Activate();
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("Название: " + dataGridView5.SelectedRows[0].Cells[1].Value.ToString()
              + "\n\nСостав: " + dataGridView5.SelectedRows[0].Cells[2].Value.ToString()
              + "\n\nОписание: " + dataGridView5.SelectedRows[0].Cells[3].Value.ToString()
              + "\n\nКатегория: " + dataGridView5.SelectedRows[0].Cells[4].Value.ToString(), "Информация");
        }

       

        private void FindRecButton_Click(object sender, EventArgs e)
        {
            if (dataGridView4.RowCount > 0)
            {

                var result = from Rec in db.Recipes
                             where Rec.Name.StartsWith(FindRecBox.Text) || Rec.Composition.StartsWith(FindRecBox.Text) 
                             || Rec.Description.StartsWith(FindRecBox.Text) || Rec.Category.StartsWith(FindRecBox.Text)
                             select new
                             {
                                 Id = Rec.Id,
                                 Name = Rec.Name,
                                 Composition = Rec.Composition,
                                 Description = Rec.Description,
                                 Category = Rec.Category,
                                 idDishes = Rec.idDishes

                             };
                dataGridView4.DataSource = result.ToList();
              
            }
        }

        private void RecAllButton_Click(object sender, EventArgs e)
        {
            db.Recipes.Load();
            dataGridView4.DataSource = db.Recipes.Local.ToBindingList();
            FindRecBox.Text = "";
        }

        private void копироватьРецептВБуферToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string k = "Состав:\n" + dataGridView4.SelectedRows[0].Cells[2].Value.ToString();
            string o = "\nОписание:\n" + dataGridView4.SelectedRows[0].Cells[3].Value.ToString();
            Clipboard.SetText(k + o);
            MessageBox.Show("Рецепт скопирован в буфер обмена", "Рецепты", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void копироватьРецептВБуферToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string k = "Состав:\n" + dataGridView5.SelectedRows[0].Cells[2].Value.ToString();
            string o = "\nОписание:\n" + dataGridView5.SelectedRows[0].Cells[3].Value.ToString();
            Clipboard.SetText(k + o);
            MessageBox.Show("Рецепт скопирован в буфер обмена", "Рецепты", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
