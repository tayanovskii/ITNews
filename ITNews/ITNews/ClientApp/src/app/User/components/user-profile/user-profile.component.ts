import { UserProfileService } from './../../services/user-profile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  profile: UserProfile;
  userName: string;
  userId: string;
  constructor(
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private userProfileService: UserProfileService
  ) {
    // this.profile = <UserProfile>{};
  }

  ngOnInit() {
    this.userId = this.authService.getUserId();
    this.userName = this.authService.getUserName();
    if (this.userId) {
      this.userProfileService.getUserProfileByUser(this.userId)
        .subscribe(res => {
          this.profile = res;
          console.log('Got profile from server: ' + JSON.stringify(this.profile));
        }, error => console.log(error));
    }
  }

}
