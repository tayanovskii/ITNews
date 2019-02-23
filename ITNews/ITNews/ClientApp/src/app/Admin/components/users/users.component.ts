import { AuthService } from 'src/app/Shared/services/auth.service';
import { ManageUserQuery } from './../../models/ManageUserQuery';
import { Component, OnInit } from '@angular/core';
import {
  faUserMinus,
  faUsers,
  faUserLock,
  faUserEdit,
  faSortAlphaDown,
  faSortAlphaUp,
  faSortAmountUp,
  faSortAmountDown } from '@fortawesome/free-solid-svg-icons';
import { faTrashAlt } from '@fortawesome/free-regular-svg-icons';
import { Router } from '@angular/router';
import { ManageUser } from '../../models/ManageUser';
import { ManageUserService } from '../../services/manage-user.service';
import { RolesService } from '../../services/roles.service';
import { forkJoin } from 'rxjs';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
users: ManageUser[];
  roles: string[];
  query: ManageUserQuery;
  pageSizeArray = [10, 15, 20, 25, 30];
  totalItems = 100;
  filterBy: string;

  // icons:
  trashIcon = faTrashAlt;
  deleteUserIcon = faUserMinus;
  blockUserIcon = faUserLock;
  editUserIcon = faUserEdit;
  viewUserIcon = faUsers;
  sortAlphaDown = faSortAlphaDown;
  sortAlphaUp = faSortAlphaUp;
  sortAmountUp = faSortAmountUp;
  sortAmountDown = faSortAmountDown;

  constructor(
    private userService: ManageUserService,
    private roleService: RolesService,
    private router: Router,
    private _modalService: NgbModal,
    private authService: AuthService
  ) {
    this.users = [];
    this.users.forEach(user => {
      user.roles = [];
    });
    // this.pageSizeArray = ArrayHelpers.array_range(15, 16);
    this.query = <ManageUserQuery> {
      isSortAscending: true,
      page: 1,
      pageSize: 10,
      sortBy: 'UserName'
    };
  }

  ngOnInit() {
    forkJoin(this.userService.getUsersByQuery(this.query), this.roleService.getRoles())
    .subscribe(res => {
      // console.log(JSON.stringify(res));
      this.users = res[0].items;
      this.totalItems = res[0].totalItems;
      this.roles = res[1];
    }, error => console.log(error))
  }
  changePageList(newPage: number) {
    this.query.page = newPage;
     this.updateUserList();
  }
  changePageSize(newPageSize) {
    this.query.pageSize = newPageSize;
    this.updateUserList();
  }

  sortUsers(sortBy: string) {
    console.log(JSON.stringify(this.query));
    this.query.page = 1;
    if (this.query.sortBy === sortBy) {
      this.query.isSortAscending = !this.query.isSortAscending;
    } else {
      this.query.sortBy = sortBy;
      this.query.isSortAscending = true;
    }
    this.updateUserList();
  }
  filterUserByRole(role: string) {
    // this.resetFilter();
    this.query.role = role;
    this.query.page = 1;
    this.updateUserList();
  }
  filterUserByStatus(isBlocked: boolean) {
    // this.resetFilter();
    this.query.userBlocked = isBlocked;
    this.query.page = 1;
    this.updateUserList();
  }
  changeFilter(filterBy: string) {
    this.filterBy = filterBy;
    this.resetFilter();
  }
  resetFilter() {
    delete this.query.role;
    delete this.query.userBlocked;
    this.updateUserList();
  }

  updateUserList() {
    this.userService.getUsersByQuery(this.query)
    .subscribe(res => {
      this.users = res.items;
      this.totalItems = res.totalItems;
    }, error => console.log(error));
  }
  lockUser(userId: string) {
    const user = this.users.find(u => u.userId === userId);
    user.userBlocked  =  !user.userBlocked;
    if (user.userBlocked) {
      this.userService.lockUser(user.userId)
      .subscribe(res => console.log(`User ${user.userName} has been locked`),
        error => console.log(error));
    } else {
      this.userService.unLockUser(user.userId)
      .subscribe(res => console.log(`User ${user.userName} has been unlocked`),
        error => console.log(error));
    }
  }
  deleteUser(userId: string, content) {
    this._modalService.open(content).result.then(res => {
      console.log(res);
      if (res === 'Ok click') {
        this.userService.deleteUserProfile(userId)
        .subscribe(result => {
          console.log(`User with ${userId} id has been deleted`);
          const ind = this.users.findIndex(u => u.userId === userId);
          this.users.splice(ind, 1);
        });
        return;
      }
    });
  }

  changeRoles(userId: string, role: string) {
    const user = this.users.find(u => u.userId === userId);
    if (user.roles.includes(role)) {
      const roleInd = user.roles.findIndex(r => r === role);
      user.roles.splice(roleInd, 1);
    } else {
      user.roles.push(role);
    }
    this.userService.changeUserProfile(user)
    .subscribe(res => console.log('User has been updated'),
    error => console.log(error));
  }
}
