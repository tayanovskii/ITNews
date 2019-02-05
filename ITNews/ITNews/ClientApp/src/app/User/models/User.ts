interface User {
  userId: string;
  userProfileId: number;
  userProfile: UserProfile;
  languageId?: number;
  createdAt: Date;
  countLike: number;
}
