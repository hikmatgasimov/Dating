
import { Component, OnInit,Output, EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { Router} from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //@Input() usersFromHomeComponent:any;//recieving data from parent
  @Output() cancelRegister = new EventEmitter(); //sending data from a child tp parent
  model:any={};
  registerForm:FormGroup;
  maxDate:Date;
  validationErrors:string[]=[];
  constructor(private accountService:AccountService,
    private toastr:ToastrService,private fb:FormBuilder,private router:Router) { }
    //FormBuilder-simplize our code

  ngOnInit(): void {
    this.intitializeForm();
    this.maxDate=new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18)
  }

  /* intitializeForm(){
    this.registerForm=new FormGroup({
      username:new FormControl('Hello',Validators.required),
      password:new FormControl('',[Validators.required,Validators.minLength(4),
      Validators.maxLength(8)]),
      confirmPassword:new FormControl('',Validators.required),this.matchValues('password'))
    })
  } */
  intitializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password:['',[Validators.required,Validators.minLength(4),
        Validators.maxLength(8)]],
      confirmPassword:['',[Validators.required,this.matchValues('password')]]
    })
  }
  matchValues(matchTo:string):ValidatorFn{
  return (control:AbstractControl )=>{
     return control?.value === control?.parent?.controls[matchTo].value ? null:{isMatching:true}//access the conrol which i attach to this validator means confirm passwor
   }
  }
  register(){
  // console.log(this.model)
   this.accountService.register(this.registerForm.value).subscribe(response=>{
     this.router.navigateByUrl('/members')
     this.cancel();
   },error=>{
     this.validationErrors=error;
     
   });
  }
  cancel(){
    this.cancelRegister.emit(false);
  }
}
