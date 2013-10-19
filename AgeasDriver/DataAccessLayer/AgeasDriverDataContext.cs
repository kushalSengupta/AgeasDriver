using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Linq;

namespace DataAccessLayer
{
    public class AgeasDriverDetails: DataContext
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public const string DBConnectionString = "Data Source=isostore:/AgeasDriver.sdf";

        // Pass the connection string to the base class.
        public AgeasDriverDetails(string connectionString)
            : base(connectionString)
        { }



        // Specify a single table for the SMS data.
        public Table<JourneyDetails> tblJourneyDetails 
        {
            get
            {
                return this.GetTable<JourneyDetails>();
            }
        }

        public Table<LocationTable> tblLocation
        {
            get
            {
                return this.GetTable<LocationTable>();
            }
        }

     public Table<UserJourney> tblUserJourney
        {
            get
            {
                return this.GetTable<UserJourney>();
            }
        }

     

    }

    [Table]
    public class JourneyDetails 
    {
        // Define ID: private field, public property and database column.


        private int _JId;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int JId
        {
            get
            {
                return _JId;
            }
            set
            {
                _JId = value;
            }
        }

        private int _JourneyId;
        [Column]
        public int JourneyId
        {
            get
            {
                return _JourneyId;
            }
            set
            {
                _JourneyId = value;
            }
        }


        private string _JDate;
        [Column]
        public string JDate
        {
            get
            {
                return _JDate;
            }
            set
            {
                _JDate = value;
            }
        }

        private decimal _Last_distance;
        //[Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        [Column]
        public decimal Last_distance 
        {
            get
            {
                return _Last_distance;
            }
            set
            {
                _Last_distance = value;
            }
        }

        // Define item name: private field, public property and database column.
        private decimal _Total_distance;
        [Column]
        public decimal Total_distance
        {
            get
            {
                return _Total_distance;
            }
            set
            {
                _Total_distance = value;
            }
        }

        // Define item name: private field, public property and database column.
      

        private string _TimeDuration;
        [Column]
        public string TimeDuration
        {
            get
            {
                return _TimeDuration;
            }
            set
            {
                _TimeDuration = value;
            }
        }



       
    }

    [Table]
    public class LocationTable 
    {
        private int _LocationID;
        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int LocationID
        {
            get
            {
                return _LocationID;
            }
            set
            {
                if (_LocationID != value)
                {

                    _LocationID = value;
                   
                }
            }
        }





        private int _Journeyid;
         [Column]
        public int Journeyid 
        {
            get
            {
                return _Journeyid;
            }
            set
            {
                _Journeyid = value;
                
            }
        }

         private string _Lat;
        [Column]
         public string Lat
        {
            get
            {
                return _Lat;
            }
            set
            {
                _Lat = value;
               
            }
        }
        private string _Long;
        [Column]
        public string Long 
        {
            get
            {
                return _Long;
            }
            set
            {
                _Long = value;
            }

        }
        private string _DateTime;
        [Column]
        public string DateTime 
        {
            get
            {
                return _DateTime;
            }
            set
            {
                _DateTime = value;
               
            }

        }
       

         }

    [Table]
    public class UserJourney  
    {
        private String _Journeyid;
        [Column]
        public String Journeyid
        {
            get
            {
                return _Journeyid;
            }
            set
            {
                _Journeyid = value;

            }
        }

        private string _JDate;
        [Column]
        public string JDate
        {
            get
            {
                return _JDate;
            }
            set
            {
                _JDate = value;

            }
        }
        private string _JTime;
        [Column]
        public string JTime
        {
            get
            {
                return _JTime;
            }
            set
            {
                _JTime = value;
            }

        }

        

    }


}
