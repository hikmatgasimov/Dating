import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

/*   members$:Observable<Member[]> */
members:Member[];
pagination:Pagination;
userParams:UserParams;
user:User;
  constructor(private membersService:MembersService,private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user=user;
      this.userParams=new UserParams(user);
    })
  }

  ngOnInit(): void {
    this.loadMembers();
    //this.members$=this.membersService.getMembers()

    //this.loadMembers();
  }
  loadMembers(){
    this.membersService.getMembers(this.userParams).subscribe(response=>{
      this.members=response.result;
      this.pagination=response.pagination;
    })
  }
  pageChanged(event:any){
    this.userParams.pageNumber=event.page;
    this.loadMembers();
  }
  /* loadMembers(){
    this.membersService.getMembers().subscribe(members=>{
      this.members=members;
    })
  } */

}
