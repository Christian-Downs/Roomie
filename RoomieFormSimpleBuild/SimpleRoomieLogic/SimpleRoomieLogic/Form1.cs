using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleRoomieLogic
{
    public partial class Form1 : Form
    {
        new List<Person> people = new List<Person>();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Person you = new Person();
            you.firstName = "You";
            you.lastName = "Person";
            you.ID = 1;
            you.phoneNumber = "5188796287";
            people.Add(you);

            Person ron = new Person() { firstName = "Ron", lastName = "Rodgers", phoneNumber = "1234567890", ID = 2 };
            Person tim = new Person() { firstName = "tim", lastName = "stevenson", phoneNumber = "1234523490", ID = 3 };
            Person steve = new Person() { firstName = "steve", lastName = "timmison", phoneNumber = "35432457890", ID = 4 };
            people.Add(ron);
            people.Add(tim);
            people.Add(steve);

        }
        private void SubmitPersonButton_Click(object sender, EventArgs e)
        {
            bool idFree = true;
            foreach (Person p in people)
            {
                if(p.ID.Equals(idLikeTextBox.Text))
                {
                    MessageBox.Show("id already used");
                    idFree = false;
                    break;
                }
            }


            if (idFree)
            {
                Person person = new Person();
                person.ID = Convert.ToInt32(idTextBox.Text);
                
                person.firstName = firstNameTextBox.Text;
                person.lastName = lastNameTextBox.Text;
                person.phoneNumber = phoneNumberTextBox.Text;
                people.Add(person);
            }
            
        }

        private void displayButton_Click(object sender, EventArgs e)
        {
            personListBox.Items.Clear();
            personListBox.Items.Add("id\tfirst name\t\t last name\tphone number");
            foreach(Person person in people)
            {
                personListBox.Items.Add(person.ID+"\t"+person.firstName + "\t\t" +person.lastName + "\t\t" +person.phoneNumber);
            }
        }


        //like a person through id
        private void button1_Click(object sender, EventArgs e)
        {
                        
            foreach(Person person in people)
            {
                if (person.ID==Convert.ToInt32(idLikeTextBox.Text))
                {
                    people[0].likedPeople.Add(person);
                    MessageBox.Show("You liked " + person.firstName);
                }
            }
            idLikeTextBox.Text = "";
        }


        //show liked people
        private void button2_Click(object sender, EventArgs e)
        {
            personListBox.Items.Clear();
            personListBox.Items.Add("id\tfirst name\t\t last name\tphone number");
            foreach (Person person in people[0].likedPeople)
            {
                personListBox.Items.Add(person.ID + "\t" + person.firstName + "\t\t" + person.lastName + "\t\t" + person.phoneNumber);
            }
        }
    }

    public class Person
    {
        public string firstName, lastName, phoneNumber;
        public int ID;

        public List<Person> likedPeople = new List<Person>();
    }
}
