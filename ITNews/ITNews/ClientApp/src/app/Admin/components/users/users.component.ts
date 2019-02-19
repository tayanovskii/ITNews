import { Component, OnInit } from '@angular/core';
import { faUserMinus, faUsers, faUserLock, faUserEdit } from '@fortawesome/free-solid-svg-icons';
import { faTrashAlt } from '@fortawesome/free-regular-svg-icons';
import { Router } from '@angular/router';
import { ManageUser } from '../../models/ManageUser';
import { ManageUserService } from '../../services/manage-user.service';
import { RolesService } from '../../services/roles.service';
import { forkJoin } from 'rxjs';
import { Role } from '../../models/Role';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users: ManageUser[];
  roles: Role[];
  // icons:
  trashIcon = faTrashAlt;
  deleteUserIcon = faUserMinus;
  blockUserIcon = faUserLock;
  editUserIcon = faUserEdit;
  viewUserIcon = faUsers;

  constructor(
    private userService: ManageUserService,
    private roleService: RolesService,
    private router: Router
  ) {
    this.users = [];
    this.users.forEach(user => {
      user.roles = [];
    });
  }

  ngOnInit() {
    forkJoin(this.userService.getUsers(), this.roleService.getRoles())
    .subscribe(res => {
      // console.log(JSON.stringify(res));
      this.users = res[0],
      this.roles = res[1]
    }, error => console.log(error))

  }

}
