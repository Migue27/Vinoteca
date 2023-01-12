using Vinoteca.Models;

namespace Vinoteca.Utilities
{
    public class ImagesController
    {
        public string Path { 
            get { return Path; } 
            set 
            {
                Path = "~/Images/UserProfile/" + this;          
            } 
        }
        public string Name { get; set; }

        public ImagesController(string path, string name)
        {
            Path = path;
            Name = name;
        }

        public void FindOrCreateFolder(ApplicationUser user)
        {

        }

    }
}
