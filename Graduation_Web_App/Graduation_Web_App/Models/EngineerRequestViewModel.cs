namespace Graduation_Web_App.Models
{
    public class EngineerRequestViewModel
    {
        public EngineerImageDto EngineerImageDto { get; set; }
        public int FarmerId { get; set; }
        public int EngineerId {  get; set; }
        public int CompanyId {  get; set; }
        public DateTime ServiceDate { get; set; }
        public double ServicePrice { get; set; }
    }
}
