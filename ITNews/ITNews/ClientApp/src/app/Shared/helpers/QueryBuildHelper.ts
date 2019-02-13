export  class  QueryBuildHelper {
  public static getQuery(queryObj: Object) {
    return Object
      .keys(queryObj)
      .map(k => encodeURIComponent(k) + '=' + encodeURIComponent(queryObj[k]))
      .join('&');
  }
}
