import { Component } from '@angular/core';
import { Group } from '../../group';
import { GroupService } from '../../group.service';
import { UserService } from '../../UserService';
import { FriendsService } from '../../Friends.service';
import { Friends } from '../../Friends';
import { User } from '../../User';
import { InvitesService } from '../../invites.service';
import { ActivatedRoute } from '@angular/router';
import { RecipeService } from '../../RecipeService';
import { Recipe } from '../../Recipe';

@Component({
  selector: 'app-detail-group',
  templateUrl: './detail-group.component.html',
  styleUrls: ['./detail-group.component.css'],
  providers: [GroupService, UserService, FriendsService, InvitesService, RecipeService]
})

/** detail-group component*/
export class DetailGroupComponent {
  userInfo: string = "";
  userId: number;
  groupList: Group[] = [];
  fList: Friends[] = [];
  userList: User[] = [];
  allUsers: User[]
  message: string | null = null;
  gList: User[] = [];
  id: number;
  group: Group;
  groupName: string;
  groupUsers: Group[];
  i: number;
  recipes: Recipe[];
  allRecipes: Recipe[] = [];

  Users: User[] = [];

  constructor(private groupService: GroupService, private UserService: UserService,
    private friendsService: FriendsService, private invitesService: InvitesService, private route: ActivatedRoute, private recipeServices: RecipeService) {
    this.id = + this.route.snapshot.paramMap.get('id');
    this.UserService.leaderboard().subscribe((User) => {
      this.allUsers = User;
    })
  }

  ngOnInit(): void {
    this.getGroup(this.id);
    /*this.getGroup(this.id);*/
    //this part is not working
    //this.getAllUsersInGroup();
    //for (this.i = 0; this.i <= this.groupUsers.length; this.i++) {
    //  this.getRecipes(this.groupUsers[this.i])
    /* }*/
  }

  //Getting the group that they clicked on, works
  getGroup(id: number) {
    this.groupService.getGroupByGroupId(id).subscribe((G) => {
      this.group = G;
      console.log(this.group);
      this.getAllUsersInGroup();
    });
  }

  // we need to go through the group table and find the user with that group name and put it in a list
  getAllUsersInGroup() {
    this.groupName = this.group.groupName;
    this.groupService.getUsersInGroup(this.groupName).subscribe((result) => {
      //working
      this.groupUsers = result;
      console.log(this.groupUsers);
      for (this.i = 0; this.i <= this.groupUsers.length; this.i++) {
        this.UserService.getUserbyId(this.groupUsers[this.i].userId).subscribe((result2) => {
          this.Users.push(result2);
        })
        this.getRecipes(this.groupUsers[this.i])
      }
    })

  }

  // we are going to go through the list(which holds all the users for the group and find their recipes) 
  // and send it through this function which will get all their recipes, and then put it into a another
  // recipe list (which will be the one displayed)
  getRecipes(g: Group) {
    this.recipeServices.displayUserRecipes(g.userId).subscribe((result) => {
      this.recipes = result;
      console.log(this.recipes);
      for (this.i = 0; this.i <= this.recipes.length; this.i++) {
        this.allRecipes.push(this.recipes[this.i])
        console.log(this.allRecipes);
        if (this.i === this.recipes.length - 1) {
          this.sortRecipes();
        }
      }
    })
  }

  sortRecipes() {
    let list = this.allRecipes.sort((a, b) => (a.score < b.score ? 1 : -1));
    this.allRecipes = list;
  }
}


