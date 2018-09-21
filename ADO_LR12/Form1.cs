using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADO_LR12
{
    public partial class Form1 : Form
    {
        PublishingHouseEntities db;
        public Form1()
        {
            InitializeComponent();
            db = new PublishingHouseEntities();
            var books = from b in db.Books
                        join a in db.Authors on b.ID_AUTHOR equals a.ID_AUTHOR
                        select new { Titel = b.NameBook, Name = a.FirstName, Surname = a.LastName, Price = b.Price};
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = books.ToList();
            trackBar1.Value = (int)db.Books.Max(b => b.Price);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (radioButton_Name.Checked)
            {
                var books = from b in db.Books
                            join a in db.Authors on b.ID_AUTHOR equals a.ID_AUTHOR
                            where a.FirstName.Contains(textBox1.Text)
                            select new { Titel = b.NameBook, Name = a.FirstName, Surname = a.LastName, Price = b.Price };
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = books.ToList();

            }
            else
            {
                var books = from b in db.Books
                            join a in db.Authors on b.ID_AUTHOR equals a.ID_AUTHOR
                            where a.LastName.Contains(textBox1.Text)
                            select new { Titel = b.NameBook, Name = a.FirstName, Surname = a.LastName, Price = b.Price };
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = books.ToList();
            }
            
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox2.Text = trackBar1.Value.ToString();
            if (radioButton_Name.Checked)
            {
                var books = from b in db.Books
                            join a in db.Authors on b.ID_AUTHOR equals a.ID_AUTHOR
                            where a.FirstName.Contains(textBox1.Text) && b.Price <= trackBar1.Value
                            select new { Titel = b.NameBook, Name = a.FirstName, Surname = a.LastName, Price = b.Price };
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = books.ToList();

            }
            else
            {
                //var books = from b in db.Books
                //            join a in db.Authors on b.ID_AUTHOR equals a.ID_AUTHOR
                //            where a.LastName.Contains(textBox1.Text) && b.Price <= trackBar1.Value
                //            select new { Titel = b.NameBook, Name = a.FirstName, Surname = a.LastName, Price = b.Price };

                var books = db.Books.Join(
                    db.Authors, //что присоеденяем
                    b => b.ID_AUTHOR, //по какому полю связь по первой таблице
                    a => a.ID_AUTHOR, //по какому полю связь по второй таблице
                    (b, a) => new { Titel = b.NameBook, Name = a.FirstName, Surname = a.LastName, Price = b.Price }) //что выбираем (какие поля будут в результате)
                    .Where(j => j.Surname.Contains(textBox1.Text) && j.Price <= trackBar1.Value); //условие

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = books.ToList();
            }
        }
    }
}
