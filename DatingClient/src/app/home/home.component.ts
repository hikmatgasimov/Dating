import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  registerMode = false;
 
  constructor() {}

  ngOnInit(): void {
    //this.registerMode=true;
    //this.getUsers();
  }
  registerToggle() {
    this.registerMode = !this.registerMode;
  }
   /* getUsers() {
    this.http
      .get('https://localhost:44340/api/users').
      subscribe(users=>this.users=users);
  } */
  cancelRegisterMode(event:boolean){
   this.registerMode=event;
  }
}

