import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  user: User;
  constructor(
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {

  }

}
