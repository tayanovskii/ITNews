import { UserProfileService } from './../../services/user-profile.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { Component, OnInit } from '@angular/core';
import { faUndoAlt } from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {
  profile: UserProfile;
  userName: string;
  role: string;
  email: string;
  backIcon = faUndoAlt;
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
    const userInfo = this.authService.getDecodeToken();
    this.userId = userInfo.sub;
    this.userName = userInfo.unique_name;
    this.role = userInfo.role;
    this.email = userInfo.email;

    if (this.userId) {
      this.userProfileService.getUserProfileByUser(this.userId)
        .subscribe(res => {
          this.profile = res;
          console.log('Got profile from server: ' + JSON.stringify(this.profile));
        }, error => console.log(error));
    }
  }
  saveProfile() {
    console.log('New Value for FirstName: ' + this.profile.firstName);
    this.userProfileService.changeUserProfile(this.profile, this.profile.id)
    .subscribe(res => {
      console.log('Updated profile -> ' + JSON.stringify(this.profile));
    }, error => console.log(error));
  }
}
