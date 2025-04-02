using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beauty_Salon.ViewModels
{

    public class SpecialistsCardViewModel : BaseViewModel
    {
        public SpecialistsViewModel SpecialistsViewModel { get; set; }
        public GradesViewModel GradesViewModel { get; set; }

        public SpecialistsCardViewModel()
        {
            SpecialistsViewModel = new SpecialistsViewModel();
            GradesViewModel = new GradesViewModel(SpecialistsViewModel);
        }
    }


}
