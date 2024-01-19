namespace Models.DTOs.user
{
    public class userSearchDto : QueryParameters
    {
        /// <summary>
        /// Get for departments
        /// </summary>
        public string[] departments { get; set; }

        /// <summary>
        /// Array de IDs de los departamentos a filtrar
        /// </summary>
        public string[] subDepartments { get; set; }

        /// <summary>
        /// Rol a filtrar, si se manda vació por  default tomará [Proveedor]
        /// </summary>
        public string role { get; set; }
    }
}