namespace dotnet2_1WebAPI
{
    /* 
     * This class don't use for Table. It will use only for AuthToken Data as Object
     */
    public class TokenData
    {
        public string Sub = "";  //Required Field, Used for core JWT
        public string Jti = ""; //Required Field, Used for core JWT
        public string Iat = ""; //Required Field, Used for core JWT
        public string UserID = "";
        public string LoginType = "";
        public string Userlevelid = "";
    }
}