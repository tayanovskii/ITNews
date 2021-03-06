import { AuthInterceptor } from './../../../Shared/services/auth.interceptor';
import { PhotoService } from './../../../Shared/services/photo-service';
import { Component, OnInit } from '@angular/core';
import { UserProfileService } from '../../services/user-profile.service';
import { ActivatedRoute } from '@angular/router';
import { faUserMd, faCalendarAlt, faDatabase } from '@fortawesome/free-solid-svg-icons';
import { NgbDatepickerConfig, NgbDateNativeAdapter, NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from 'src/app/Shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-profile',
  templateUrl: './create-profile.component.html',
  styleUrls: ['./create-profile.component.css'],
  providers: [
    NgbDatepickerConfig,
    {
      provide: NgbDateAdapter,
      useClass: NgbDateNativeAdapter
    }]
})
export class CreateProfileComponent implements OnInit {
  profile: UserProfile;
  editMode: boolean;
  // icons
  specializationIcon = faUserMd;
  calendarIcon = faCalendarAlt;
  saveIcons = faDatabase;
  testDate: Date = new Date;

  constructor(
    private profileService: UserProfileService,
    private activatedRoute: ActivatedRoute,
    private config: NgbDatepickerConfig,
    private photoService: PhotoService,
    private authService: AuthService,
    private toastr: ToastrService
  ) {
    this.profile = <UserProfile>{
      userId: authService.getUserId()
    };
    config.minDate = {
      day: 1,
      month: 1,
      year: 1920
    };
  }
  ngOnInit(): void {
    this.editMode = this.activatedRoute.snapshot.url[0].path.includes('edit') || this.activatedRoute.snapshot.url[1].path.includes('edit');
    if (this.editMode) {
      this.profileService.getUserProfileByUser(this.profile.userId)
        .subscribe(res => {
            this.profile = res;
            console.log(JSON.stringify(this.profile.birthDay));
          },
          error => console.log(error));
    }
  }
  saveProfile() {
    console.log('Saving this profile', JSON.stringify(this.profile));

    if (this.editMode) {
      this.profileService.changeUserProfile(this.profile, this.profile.id)
        .subscribe(res => {
            console.log('Profile has been successfully edited' + JSON.stringify(res));
          },
          error => console.log(error));
    } else {
      this.profileService.createUserProfile(this.profile)
        .subscribe(res => {
          //this.toastr.success('Profile saved!', 'Creation Profile Detail!');
          console.log('Profile has been successfully created' + JSON.stringify(res));
        }, error => console.log(error));
    }
    
  }

  uploadPhoto(photo) {
    console.log('photo has been downloaded for user' + this.profile.userId);
    this.photoService.createAvatar(photo.files[0], this.profile.userId)
    .subscribe(res => {
      console.log('file was downloaded to path: ' + res.fileName);
      this.profile.avatar = res.fileName;
    }, error => console.log(error));
  }
}
