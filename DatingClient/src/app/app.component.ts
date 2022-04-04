import { Component } from '@angular/core';
import{ HttpClient} from  '@angular/common/http';
import { OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Client';
  users:any
  //private http :HttpClient,

  constructor(private accountService: AccountService){
  }

  ngOnInit(){
   this.setCurrentUser();
  //  this.getUsers();
  }
  setCurrentUser(){
    const user:User=JSON.parse(localStorage.getItem('user')|| '{}');
  }
 /*  getUsers(){
    this.http.get('https://localhost:44340/api/users')
    .subscribe(response=>{
      this.users=response;
    },error=>{
      console.log(error);
    })
  } */


}
