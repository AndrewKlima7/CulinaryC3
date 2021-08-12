﻿using CulinaryC3.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CulinaryController : ControllerBase
    {
        //Create a Database object
        CookBook2Context db = new CookBook2Context();

        [HttpGet("recipeofday")]
        public RecipeofDay RecipeOfDay()
        {
            int day = DateTime.Today.Day + 1;
            List<Recipe> r = db.Recipes.ToList();

            Random ran = new Random();
            int num = ran.Next(0, r.Count);


            RecipeofDay d = new RecipeofDay();

            d.recipeId = r[num].Id;
            d.dayofMonth = day;

            return d;
        }


        //if they 'complete' someone else recipe
        [HttpPut("completed/u={id}")]
        public void Completed(int id)
        {
            User u = db.Users.Find(id);
            //this is an honor system we hope no one exploits it lol

            u.Score = u.Score + 5;


            //posting a recipe will be 20
            db.Users.Update(u);
            db.SaveChanges();
        }


        //-------------- LIST ALL Recipes --------------------
        [HttpGet("Recipe")]
        public List<Recipe> GetRecipes()
        {
            List<Recipe> recipeList = db.Recipes.ToList();
            return recipeList;
        }

        //------------- USERs ------------------
        //When you register a new user, grab the email from Identity and add it to a users object
        [HttpPost("GetEmail/e={email}&p={password}")]
        public void AddUser(string email, string password)
        {
            string newPass = Encrypt(password);

            User u = new User();

            u.LoginId = email;
            u.Password = newPass;

            u.Name = null;
            u.Score = 0;

            db.Users.Add(u);
            db.SaveChanges();
        }

        [HttpPost("pw={password}&e={email}/check")]
        public bool login(string password, string email)
        {
            bool login;
            string npass = Encrypt(password);
            try
            {
                User u = db.Users.Where(x => x.LoginId.ToLower() == email.ToLower() && x.Password == npass).ToList().First();
                if (u != null)
                {
                    return login = true;
                }
                return login = false;
            }
            catch (System.InvalidOperationException)
            {
                return login = false;
            }
        }

        [HttpPut("newEmail={email}&u={userId}")]
        public void NewEmail(string email, int userId) {
            User u = db.Users.Find(userId);

            u.LoginId = email;

            db.Users.Update(u);
            db.SaveChanges();
        }

        [HttpPut("Winner={userId}")]
        public void Winner(int userId)
        {
            User u = db.Users.Find(userId);

            u.Score = u.Score + 17501;

            db.Users.Update(u);
            db.SaveChanges();
        }

        [HttpDelete("removeUser={userId}")]
        public void RemoveUser(int userId)
        {
            List<Recipe> recipes = db.Recipes.Where(x => x.UserId == userId).ToList();
            foreach (Recipe r in recipes)
            {
                List<Ingredient> ing = db.Ingredients.Where(x => x.RecipeId == r.Id).ToList();
                foreach(Ingredient i in ing)
                {
                    db.Ingredients.Remove(i);
                }
            }
            foreach(Recipe r2 in recipes)
            {
                db.Recipes.Remove(r2);
            }

            List<Friend> freinds = db.Friends.Where(x => x.UserId == userId).ToList();
            foreach(Friend f in freinds)
            {
                db.Friends.Remove(f);
            }

            List<Group> groups = db.Groups.Where(x => x.UserId == userId).ToList();
            foreach (Group g in groups)
            {
                db.Groups.Remove(g);
            }

            List<Favorite> favs = db.Favorites.Where(x => x.UserId == userId).ToList();
            foreach (Favorite fav in favs)
            {
                db.Favorites.Remove(fav);
            }

            User u = db.Users.Find(userId);
            db.Users.Remove(u);

            db.SaveChanges();
        }

        [HttpPut("newPass={password}&u={userId}")]
        public void newPassword(string password, int userId)
        {
            string npass = Encrypt(password);
            User u = db.Users.Find(userId);

            u.Password = npass;

            db.Users.Update(u);
            db.SaveChanges();
        }

        [HttpGet("pw={input}")]
        public string Encrypt(string input)
        {
            Random r = new Random(input[0]);
            List<char> output = new List<char>();
            int minLength = 7;
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                c = (char)(c + r.Next(-26, 27));
                output.Add(c); 
            }
            if (output.Count < minLength)
            {
                for (int i = output.Count; output.Count < 15; i++)
                {
                    string let = Char.ConvertFromUtf32(r.Next(65, 123));
                    output.Add(let[0]);

                }
            }
            return new string(output.ToArray());
        }

        [HttpPut("img={img}&u={id}")]
        public void changeAvatar(string img, int id)
        {
            //adds 5 points if its the first time changing name
            User u = db.Users.Find(id);
            if (u.Picture == null)
            {
                u.Score = u.Score + 5;
            }

            u.Picture = img;

            db.Users.Update(u);
            db.SaveChanges();

        }

        [HttpPut("title={title}&u={id}")]
        public void changeTitle(string title, int id)
        {
            //adds 5 points if its the first time changing name
            User u = db.Users.Find(id);
            if (u.Title == null)
            {
                u.Score = u.Score + 5;
            }

            u.Title = title;

            db.Users.Update(u);
            db.SaveChanges();

        }

        [HttpPut("newname={name}&id={id}")]
        public void UpdateName(string name, int id)
        {
            //adds 5 points if its the first time changing name
            User u = db.Users.Find(id);
            if (u.Name == null)
            {
                u.Score = u.Score + 5;
            }

            u.Name = name;

            db.Users.Update(u);
            db.SaveChanges();

        }

        //get all users
        [HttpGet("Leaderboard")]
        public List<User> GetUsers()
        {
           return db.Users.OrderByDescending(o=>o.Score).ToList();
        }

        [HttpGet("UserId={id}")]
        public User GetUsersById(int id)
        {
            User u = db.Users.Where(x => x.Id == id).ToList().First();
            return u;
        }

        [HttpGet("name={name}")]
        public List<User> GetUsersByName(string name)
        {
            List<User> users = db.Users.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
            return users;
        }

        [HttpGet("Login={email}")]
        public User GetUsersById(string email)
        {
            User u = db.Users.Where(x => x.LoginId.ToLower() == email.ToLower()).ToList().First();
            return u;
        }

        //-----------------Group-------------------------------
        //display group to a user
        //displaying own groups
        [HttpGet("GetGroup/userId={id}")]
        public List<Group> GetGroupsByUser(int id)
        {

            List<Group> groups = new List<Group>();

            groups = db.Groups.Where(x => x.UserId == id).ToList();

            return groups;
        }

        [HttpGet("Allgroups")]
        public List<Group> Groups()
        {
            List<Group> g = db.Groups.ToList();
            return g;
        }

        [HttpGet("GetGroup/GroupName={name}")]
        public List<Group> GetGroups(string name)
        {

            List<Group> groups = new List<Group>();

            groups = db.Groups.Where(x => x.GroupName.ToLower() == name.ToLower()).ToList();

            return groups;
        }

        [HttpGet("GetGroup/Id={id}")]
        public Group GetGroupById(int id)
        {
            Group g = db.Groups.Where(x => x.UserId == id).ToList().First();

            return g;
        }

        [HttpGet("GetGroupByGroupId/Id={id}")]
        public Group GetGroupByGroupId(int id)
        {
            Group g = db.Groups.Where(x => x.GroupId == id).ToList().First();

            return g;
        }

        //--------------- Need to test This!! ------------------
        //Add user to a group
        //on accepting an invitation
        //for joining
        [HttpPost("AddUserToGroup/id={id}&gn={groupName}")]
        public void AddUserToGroup(int id, string groupName)
        {
            //create a new group object
            Group g = new Group();
            //find a user by the email
            User u = db.Users.Find(id);

            //if statement if a list base on group name is = 0

            //you can only join 5 groups at a time
            List<Group> groups = db.Groups.Where(x => x.UserId == u.Id).ToList();
            if (groups.Count < 5)
            {
                //adding 5 points for joing a group
                u.Score = u.Score + 5;
                db.Users.Update(u);

                //get the user id to the group id

                g.UserId = u.Id;
                //update groupName through paremeter
                g.GroupName = groupName;
                g.Admin = false;
                //update and save changes to DB
                db.Groups.Add(g);
                db.SaveChanges();
            }

        }

        //to check if user is already in the group on the front end
        [HttpGet("checkgroup/u={userid}&n={name}")]
        public List<Group> CheckGroup(int userid, string name)
        {
            List<Group> g = db.Groups.Where(x => x.UserId == userid && x.GroupName == name).ToList();
            return g;
        }

        [HttpGet("gname={name}")]
        public List<Group> GetGroupsByName(string name)
        {
            List<Group> g = db.Groups.Where(x => x.GroupName == name).ToList();
            return g;
        }

        [HttpGet("getUsersInGroup/GroupName={name}")]

        public List<Group> GetUsersInGroup(string name)
        {
            List<Group> groupUsers = db.Groups.Where(x => x.GroupName == name).ToList();
            return groupUsers;
        }


        //creating group
        //adding group will be on the profile page
        //inside the group page is where you can find the button to invite users
        [HttpPost("CreateGroup/gname={name}&userId={id}")]
        public void CreateGroup(string name, int id)
        {
            Group g = new Group();
            g.GroupName = name;
            g.UserId = id;
            g.Admin = true;

            User u = db.Users.Find(id);
            List<Group> groups = db.Groups.Where(x => x.GroupName == name).ToList();

            List<Group> userGroups = db.Groups.Where(x => x.UserId == id).ToList();
            if (groups.Count == 0)
            {
                //if they are in five groups they cannot make a group
                if (userGroups.Count < 5)
                {
                    u.Score = u.Score + 10;

                    db.Users.Update(u);
                    db.Groups.Add(g);
                    db.SaveChanges();
                }
            }
        }

        //delete user from group
        [HttpDelete("deleteuser={id}&n={groupName}")]
        public void RemoveUserFromGroup(int id, string groupName)
        {
            Group groups = db.Groups.Where(x => x.GroupName == groupName && x.UserId == id).ToList().First();

            db.Groups.Remove(groups);
            db.SaveChanges();
        }

        //only viewable to the user who created the group
        [HttpDelete("removegroup/gname={name}")]
        public void RemoveGroup(string name)
        {
            List<Group> groups = db.Groups.Where(x => x.GroupName.ToLower() == name.ToLower()).ToList();
            foreach (Group g in groups)
            {
                db.Groups.Remove(g);
            }
            db.SaveChanges();
        }

        //------------------------Favorites-----------------------------------

        [HttpGet("userfavorites={id}")]
        public List<Recipe> GetFavorites(int id)
        {
            List<Recipe> rList = db.Recipes.ToList();

            List<Favorite> favorites = db.Favorites.Where(x => x.UserId == id).ToList();
            List<Recipe> recipes = new List<Recipe>();

            foreach (Favorite f in favorites)
            {
                foreach (Recipe r in rList)
                {
                    if (f.RecipeId == r.Id)
                    {
                        recipes.Add(r);
                    }
                }
            }
            return recipes;
        }

        [HttpGet("checkFavs={userId}&f={recipeId}")]
        public List<Favorite> CheckFavorites(int userId, int recipeId)
        {
            List<Favorite> f = db.Favorites.Where(x => x.UserId == userId && x.RecipeId == recipeId).ToList();
            return f;
        }

        [HttpPost("addfav/u={userid}&r={recipeid}")]
        public void AddFavorite(int userid, int recipeid)
        {
            Favorite f = new Favorite();
            f.RecipeId = recipeid;
            f.UserId = userid;

            db.Favorites.Add(f);
            db.SaveChanges();
        }

        [HttpDelete("removefav/u={userid}&r={recipeid}")]
        public void RemoveFavorite(int userid, int recipeid)
        {
            Favorite f = new Favorite();
            f.RecipeId = recipeid;
            f.UserId = userid;

            List<Favorite> fav = db.Favorites.Where(x => x.RecipeId == f.RecipeId && x.UserId == f.UserId).ToList();

            foreach(Favorite f2 in fav)
            {
                db.Favorites.Remove(f2);
            }
            
            db.SaveChanges();
        }

        //----------------------FriendsList---------------
        [HttpGet("friends={id}")]
        public List<User> GetFriends(int id)
        {
            List<User> uList = db.Users.ToList();

            List<Friend> friends = db.Friends.Where(x => x.UserId == id).ToList();
            List<User> users = new List<User>();

            foreach (Friend f in friends)
            {
                foreach (User r in uList)
                {
                    if (f.FriendId == r.Id)
                    {
                        users.Add(r);
                    }
                }
            }
            return users;
        }

        [HttpGet("Allfriends")]
        public List<Friend> AllFriends()
        {
            List<Friend> f = db.Friends.ToList();
            return f;
        }

        //this to get friends data and display that that way I can ensure that if the user already
        //has someone as a friend the button wont even show to add friend!
        [HttpGet("checkfriends={userId}/f={friendId}")]
        public List<Friend> checkFriends(int userId, int friendId)
        {
            List<Friend> f = db.Friends.Where(x => x.UserId == userId && x.FriendId == friendId).ToList();
            return f;
        }

        //maybe this would be easier with an email and then finding the user(friend) to that email
        [HttpPost("newfriend/u={userid}&f={friendid}")]
        public void AddFriend(int userid, int friendid)
        {
            
            User u = db.Users.Find(userid);

            u.Score = u.Score + 5;

            //checking if they are already friends and then not adding it if they are friends
            Friend f = new Friend();
            f.UserId = userid;
            f.FriendId = friendid;

                db.Users.Update(u);
                db.Friends.Add(f);
                db.SaveChanges();


        }

        [HttpDelete("removefriend/u={userid}&f={friendid}")]
        public void RemoveFriend(int userid, int friendid)
        {
            Friend f = new Friend();
            f.FriendId = friendid;
            f.UserId = userid;

            Friend f2 = db.Friends.Where(x => x.FriendId == friendid && x.UserId == userid).ToList().First();

            db.Friends.Remove(f2);
            db.SaveChanges();
        }



        //------------------------------------Invites----------------------------------------------------------

        [HttpGet("Invites/U={userId}")]
        public List<Invite> GetInvites(int userId)
        {
            List<Invite> invites = db.Invites.Where(x => x.InviteeId == userId).ToList();
            return invites;
        }


        //this will be called for both not accepting and accepting an invite as for either one you no longer want to see it
        [HttpDelete("removeI={name}&u={inviteeId}")]
        public void RemoveInvite(string name, int inviteeId)
        {
            List<Invite> I = db.Invites.Where(x => x.InviteeId == inviteeId && x.NameofGroup == name).ToList();
            foreach(Invite In in I)
            {
                db.Invites.Remove(In);
            }
            
            db.SaveChanges();
        }

        //this happens when someone sends an invite the email is coming from the authorize.service in the front-end
        //name will come from the group as they select the group they want to
        //invite someone to before sending the invite
        [HttpPost("NewInvite/U={inviteeId}&O={inviterEmail}&N={name}")]
        public void NewInvite(int inviteeId, string inviterEmail, string name)
        {
            Invite I = new Invite();
            I.InviteeId = inviteeId;
            I.InviterEmail = inviterEmail;
            I.NameofGroup = name;

            db.Invites.Add(I);
            db.SaveChanges();
        }
    }
}

