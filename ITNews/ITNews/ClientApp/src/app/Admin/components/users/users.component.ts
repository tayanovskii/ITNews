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
  lockUser(userId: string) {
    let user = this.users.find(u => u.userId === userId);
    user.userBlocked  =  !user.userBlocked;
    this.userService.changeUserProfile(user)
    .subscribe(res => console.log(`User ${user.userName} has been updated`),
      error => console.log(error));
  }
  deleteUser(userId: string) {
    if (confirm('Are you sure you want to delete user?')) {
      this.userService.deleteUserProfile(userId)
      .subscribe(res =>
        {
          console.log(`User with ${userId} id has been deleted`);
          const ind = this.users.findIndex(u => u.userId === userId);
          this.users.splice(ind, 1);
        });
    }
  }
  changeRoles(userId: string, role: Role) {
    const user = this.users.find(u => u.userId === userId);
    if (user.roles.includes(role.name)) {
      const roleInd = user.roles.findIndex(r => r === role.name);
      user.roles.splice(roleInd, 1);
    } else {
      user.roles.push(role.name);
    }
    this.userService.changeUserProfile(user)
    .subscribe(res => console.log('User has been updated'),
    error => console.log(error));
  }
}
