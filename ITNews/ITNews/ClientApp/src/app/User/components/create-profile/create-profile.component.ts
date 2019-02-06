import { Component, OnInit } from '@angular/core';
import { UserProfileService } from '../../services/user-profile.service';
import { ActivatedRoute } from '@angular/router';
import { faUserMd, faCalendarAlt, faDatabase } from '@fortawesome/free-solid-svg-icons';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-create-profile',
  templateUrl: './create-profile.component.html',
  styleUrls: ['./create-profile.component.css'],
  providers: [NgbDatepickerConfig]
})
export class CreateProfileComponent implements OnInit {
  profile: UserProfile;
  // icons
  specializationIcon = faUserMd;
  calendarIcon = faCalendarAlt;
  saveIcons = faDatabase;

  constructor(
    private profileService: UserProfileService,
    private activatedRoute: ActivatedRoute,
    private config: NgbDatepickerConfig
  ) {
    this.profile = <UserProfile>{};
    config.minDate = {
      day: 1,
      month: 1,
      year: 1920
    };
  }
  ngOnInit(): void {
  }
  saveProfile() {
    console.log(JSON.stringify(this.profile));
  }
}
