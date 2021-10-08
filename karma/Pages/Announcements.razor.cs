using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using karma.Components.Dialogs;

namespace karma.Pages
{
    public partial class Announcements
    {
        private List<Announcement> _announcements;

        protected override void OnInitialized()
        {
            using (var db = new dbkarmaContext())
            {
                _announcements = db.Announcements.OrderByDescending(x => x.Added).ToList();
            }
        }

        // Define dialog
        async Task OpenDialog()
        {
            var dialog = _dialogService.Show<DialogAddNewAnnouncement>("Add announcement");
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                Announcement announcement = (Announcement)result.Data;
                announcement.Added = DateTime.Now;

                using (var db = new dbkarmaContext())
                {
                    db.Add(announcement);
                    db.SaveChanges();
                }

                _announcements.Insert(0, announcement);
            }
        }
    }
}