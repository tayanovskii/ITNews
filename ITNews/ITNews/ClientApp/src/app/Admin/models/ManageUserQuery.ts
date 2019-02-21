export class ManageUserQuery {
  userBlocked?: boolean;
  role: string;
  sortBy: string;
  isSortAscending: boolean;
  page: number;
  pageSize: number;
}
