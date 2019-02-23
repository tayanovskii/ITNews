import { UserProfileService } from './../../services/user-profile.service';
import { Router, ActivatedRoute, UrlSegment } from '@angular/router';
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
  myProfileMode: boolean;
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
    this.myProfileMode = false;
    this.activatedRoute.snapshot.url.forEach(element => {
      if (element.path === 'my-profile') {
        this.myProfileMode = true;
        return;
      }
    });
    // this.profile = <UserProfile>{};
  }

  ngOnInit() {
    let userInfo = null;
    if (this.myProfileMode) {
      userInfo = this.authService.getDecodeToken();
      this.userId = userInfo.sub;
      this.userName = userInfo.unique_name;
      this.role = userInfo.role;
      this.email = userInfo.email;
    } else {
      this.userId = this.activatedRoute.snapshot.paramMap.get('userId');
    }

    if (this.userId) {
      this.userProfileService.getUserProfileByUser(this.userId)
        .subscribe(res => {
          this.profile = res;
          console.log('Got profile from server: ' + JSON.stringify(this.profile));
          console.log('User is admin->' + this.authService.isUserAdmin());
          console.log('This profile is current user\'s profile->' + (this.authService.getUserId() === this.profile.userId));
        }, error => console.log(error));
    }
  }
  isCanChange(): boolean {
    return !(this.authService.isUserAdmin() || this.authService.getUserId() === this.profile.userId);
  }
  saveProfile() {
    console.log('New Value for FirstName: ' + this.profile.firstName);
    this.userProfileService.changeUserProfile(this.profile, this.profile.id)
    .subscribe(res => {
      console.log('Updated profile -> ' + JSON.stringify(this.profile));
    }, error => console.log(error));
  }
}
