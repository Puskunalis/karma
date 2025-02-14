﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using karma.Components.Dialogs;
using MudBlazor;
using Microsoft.EntityFrameworkCore;
using karma.Data;

namespace karma.Pages
{
    public partial class UserPage
    {
        private UserInfo _mainUser = UserInfo.GetInstance();
        private List<Announcement> _announcements;
        private List<Listing> _listings;
        private List<Message> _messages;
        private List<User> _users;
        private IEnumerable<RenderedMessage> renderedMessages;
        private IEnumerable<IGrouping<bool,User>> charityCount;

        public User getUserByID(int thisId)
        {
            using (var db = new db_a7d4c3_karmaContext())
            {
                User user = db.Users.Where(b => b.Id == thisId).FirstOrDefault();
                return user;
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            using (var db = new db_a7d4c3_karmaContext())
            {
                _announcements = await db.Announcements.OrderByDescending(x => x.Added).ToListAsync();
                _listings = await db.Listings.OrderByDescending(x => x.Added).ToListAsync();
                _messages = await db.Messages.ToListAsync();
                _users = await db.Users.ToListAsync();
            }
            //REQUIREMENT 3.10 LINQ JOIN
            renderedMessages = _messages.Join(
            _listings,
            message => message.ListingId,
            listing => listing.Id,
            (message, listing) => new RenderedMessage
            {
                Title = listing.Title,
                Description = listing.Description,
                Img = listing.Img,
                Message = message.Message1,
                Sender = getUserByID(message.UserId).Name,
                ListerId = listing.UserId
            }) ;

            //REQUIREMENT 3.10 LINQ GROUPBY
            charityCount = _users.GroupBy(e => e.IsCharity);
        }
      
        async Task OpenDialogConfirmationListing(int Id)
        {
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
            var dialog = _dialogService.Show<DialogConfirmation>("Confirmation", options);
            var result = await dialog.Result;


            if (!result.Cancelled)
            {
                var listing = new Listing { Id = Id };
                using (var db = new db_a7d4c3_karmaContext())
                {
                    db.Listings.Attach(listing);
                    db.Listings.Remove(listing);
                    db.SaveChanges();
                    _listings = await db.Listings.OrderByDescending(x => x.Added).ToListAsync();
                    NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
                }
            }
        }
        async Task OpenDialogConfirmationAnnouncement(int Id)
        {
            DialogOptions options = new DialogOptions() { MaxWidth = MaxWidth.Large, FullWidth = true };
            var dialog = _dialogService.Show<DialogConfirmation>("Confirmation", options);
            var result = await dialog.Result;


            if (!result.Cancelled)
            {
                var announcement = new Announcement { Id = Id };
                using (var db = new db_a7d4c3_karmaContext())
                {
                    db.Announcements.Attach(announcement);
                    db.Announcements.Remove(announcement);
                    db.SaveChanges();
                    _announcements = await db.Announcements.OrderByDescending(x => x.Added).ToListAsync();
                }
            }
        }
    }
    
}
