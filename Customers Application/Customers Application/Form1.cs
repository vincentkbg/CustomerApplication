//Packages and extensions required in order to run application
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//Extension added to the project in order to make json functions available
using Newtonsoft.Json;
using System.IO;
using System.Web;
using WebApiContrib.Formatting;


namespace Customers_Application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Initialisation of constructors needed to load json file into project
        public class Customers 
        {
            
            public double latitude { get; set; }
            public int user_id { get; set; }
            public string name { get; set; }
            public double longitude { get; set; }
            public double distancetooffice { get; set; }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //Conversion of office latitude and longitude from degrees to radiant
            //Conversion from radiant to degree : x = (angle *pi)/180
            double officelatitude = ((53.3381985 * Math.PI) / 180);
            double officelongitude = ((-6.2592576 * Math.PI) / 180);

            //Function reading the json file.
            //Function containing path to physical file
            string json = File.ReadAllText("D:/Visual Studio 2012/Labs/Customers Application/Customers Application/Customer List/gistfile1.txt");
            
            //Elements from json list are broken up and loaded into new list
            // Each element is added to its corresponding node in the new list where user_id are added to user_id and so on
            List<Customers> customerlist = JsonConvert.DeserializeObject<List<Customers>>(json);

            //loop reading each elements of the list and ordering each line from the list by ascending order based on the user_id
            foreach (Customers customers in customerlist.OrderBy(id=>id.user_id))
            {
                //the latitude and longitude of each customer is converted is loaded into a new variable for calculation purprise
                double customerlatitudedegree = Convert.ToDouble(customers.latitude);
                double customerlongitudedegree = Convert.ToDouble(customers.longitude);

                
                //Conversion from radiant to degree : x = (angle *pi)/180
                double customerlatituderadiant = ((customerlatitudedegree * Math.PI) / 180);
                double customerlongituderadiant = ((customerlongitudedegree * Math.PI) / 180);

                //To get the distance between 2 cordinates
                //the central angle is calculated x = arcosine ((sinus latidudex1 * sinus latitudex2) + (cosine longitudey1 * consine longitudey2 * cosine (latitudex2- longitudey2))
                double centralangle =Math.Acos (((Math.Sin(officelatitude) * Math.Sin(customerlatituderadiant) + (Math.Cos(officelongitude) * Math.Cos(customerlongituderadiant) * Math.Cos(customerlatituderadiant - customerlongituderadiant)))));
                //the distance is then calculated by multiplying the central angle to the radius of the earth which is 6371
                double distancetooffice = 6371 * centralangle;

                //the distance is then saved to the list
                customers.distancetooffice = distancetooffice;

                // if the distance is inferior or equal to 100 then the user_id and customer name is displayed else its not.
                if (distancetooffice <= 100)
                {
                    label2.Text += string.Format("user_id:{0} , name:{1} \n", customers.user_id, customers.name);
                }
   
            }
           
        }
    }
}
