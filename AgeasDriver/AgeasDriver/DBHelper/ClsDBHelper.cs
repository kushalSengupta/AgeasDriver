using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace AgeasDriver.ClsDBHelper
{
    class ClsDBHelper
    {
        public const string DBConnectionString = "Data Source=isostore:/AgeasDriver.sdf";


        public Boolean InsertLocation(LocationTable LocationDtls)
        {
            Boolean result = false;
            try
            {
                using (var context = new AgeasDriverDetails(DBConnectionString))
                {
                    if (context.DatabaseExists())
                    {

                        context.tblLocation.InsertOnSubmit(LocationDtls);
                        context.SubmitChanges();
                        result = true;
                    }
                    context.Dispose();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }


        public Boolean InsertJourney(JourneyDetails JourneyDtls)
        {
            Boolean result = false;
            try
            {
                using (var context = new AgeasDriverDetails(DBConnectionString))
                {
                    if (context.DatabaseExists())
                    {

                        context.tblJourneyDetails.InsertOnSubmit(JourneyDtls);
                        context.SubmitChanges();
                        result = true;
                    }
                    context.Dispose();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }


        public Boolean CreateDatabase()
        {
            Boolean result = false;
            try
            {
                using (var context = new AgeasDriverDetails(DBConnectionString))
                {
                    if (!context.DatabaseExists())
                    {
                        context.CreateDatabase();
                        result = true;
                    }
                    context.Dispose();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public Boolean IsDatabaseExists()
        {
            Boolean result = false;
            try
            {
                using (var context = new AgeasDriverDetails(DBConnectionString))
                {
                    if (context.DatabaseExists())
                    {
                        result = true;
                    }
                    context.Dispose();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        
        public List<LocationTable> GetlocationList(IQueryable<LocationTable> JourneyQuery)
        {
            List<LocationTable> Locationlist = null;

            try
            {
                using (var context = new AgeasDriverDetails(DBConnectionString))
                {
                    if (context.DatabaseExists())
                    {
                        Locationlist = JourneyQuery.ToList();
                    }
                    context.Dispose();
                }
            }
            catch
            {
                Locationlist = null;
            }
            return Locationlist;
        }

        public List<JourneyDetails> GetJourneyList(IQueryable<JourneyDetails> JourneyQuery)
        {
            List<JourneyDetails> Journeylist = null;

            try
            {
                using (var context = new AgeasDriverDetails(DBConnectionString))
                {
                    if (context.DatabaseExists())
                    {
                        Journeylist = JourneyQuery.ToList();
                    }
                    context.Dispose();
                }
            }
            catch
            {
                Journeylist = null;
            }
            return Journeylist;
        }

        public  Boolean DeleteLocation(IQueryable<LocationTable> QryDelLocation, AgeasDriverDetails context)
        {
            Boolean result = false;
            try
            {
                context.tblLocation.DeleteAllOnSubmit(QryDelLocation);
                context.SubmitChanges();
                //context.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    


    public  Boolean DeleteJourneyDetails(IQueryable<JourneyDetails> QryDelLocation, AgeasDriverDetails context)
        {
            Boolean result = false;
            try
            {
                context.tblJourneyDetails.DeleteAllOnSubmit(QryDelLocation);
                context.SubmitChanges();
                //context.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }
    }

}

