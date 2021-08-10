import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { User } from './User';

@Injectable()
export class UserService {
  base: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.base = baseUrl + "Culinary";
  }

  //needs to be tested still
  updateUsers(name: string, id: number) {
    let url: string = this.base + `/newname=${name}&id=${id}`;
    this.http.put(url, {}).subscribe(result => { console.log(result) });
  }

  //We use this in the detail-recipe page, it does works
  leaderboard() {
    let url: string = this.base + "/Leaderboard";
    return this.http.get<User[]>(url);
  }

  //serach by email
  getUserbyLoginId(loginId: string) {
    let url: string = this.base + `/Login=${loginId}`
    return this.http.get<User>(url);
  }

  //search by user name
  getUsersbyName(name: string) {
    let url: string = this.base + `/name=${name}`
    return this.http.get<User[]>(url);
  }


  getUserbyId(userId: number) {
    let url: string = this.base + `/UserId=${userId}`
    return this.http.get<User>(url);
  }

  completeRecipe(userId: number) {
    let url: string = this.base + `/completed/u=${userId}`;
    return this.http.put<User>(url, {}).subscribe((result) => {
      console.log(result);
    });
  }

  title(title: string, id: number) {
    let url: string = this.base + `/title=${title}&u=${id}`;
    this.http.put(url, {}).subscribe(result => { console.log(result) });
  }

  avatar(img: string, id: number) {
    let url: string = this.base + `/img=${img}&u=${id}`;
    this.http.put(url, {}).subscribe(result => { console.log(result) });
  }

  createUser(email: string, password: string) {
    let url: string = this.base + `/GetEmail/e=${email}&p=${password}`
    this.http.post<User>(url, {}).subscribe(result => { console.log(result) });
  }
}
