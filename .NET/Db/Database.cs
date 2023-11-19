using Npgsql;

using Domus.Models;

namespace Domus.Db
{
    public class Database
    {   
        RealEstateContext db = new();
        
        public void CreateRealEstateEntity(RealEstate realEstateEntity)
        {
            db.Add(realEstateEntity);
            db.SaveChanges();
        }

        public void DestroygRealEstateEntity(string finnkode)
        {
            var realEstateEntity = db.Find(typeof(RealEstate), finnkode);
            
            if (realEstateEntity != null)
            {
                var entityTracker = db.Remove(realEstateEntity);
                db.SaveChanges();
            }
        }

        public object? GetEntity(string finnkode)
        {
            var realEstateEntity = db.Find(typeof(RealEstate), finnkode);
            if (realEstateEntity != null)
            {
                return realEstateEntity;
            }

            return null;
        }  
    }

    class PostgreSQL //Legacy
    {
        NpgsqlConnection dbCon = new();
        public PostgreSQL()
        {
            try 
            { 
                this.dbCon.ConnectionString = DbConfig.DB_CON_STRING;
                this.dbCon.Open(); 
            }
            catch (Npgsql.NpgsqlException pgE)
            {
                System.Console.WriteLine(pgE.Message);
                System.Console.WriteLine(pgE.ErrorCode);
            }
              
        }

        public void CreateRealEstateInfo(Models.RealEstateContext dbContext)
        {
            if (this.dbCon.State == System.Data.ConnectionState.Open)
            {

            }
        }
        
        //Summary:
        //    Removes a Realestate record from DB, given [finnkode].     
        public void DestroyRealEstateInfo(string finnkode)
        {
             if (this.dbCon.State == System.Data.ConnectionState.Open)
            {
                
            }
        }
    }
}