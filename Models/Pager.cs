namespace aser_electrification.Models
{
    public class Pager
    {
        public int TotalItems { get; set; }

        public int CurentPage { get; set; }

        public int PageSize { get; set; }

        public int TottalPages { get; set; }

        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager() 
        { 
        }

        public Pager(int totalItems,  int page,  int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }
            if(endPage >totalPages)
            {
                endPage = totalPages;
                if(startPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totalItems;
            CurentPage = currentPage;
            PageSize = pageSize;
            TottalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
           
        }
    }
}
