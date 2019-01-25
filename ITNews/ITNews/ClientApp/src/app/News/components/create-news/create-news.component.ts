import { Category } from './../../../Shared/models/category';
import { NewsService } from './../../services/news.service';
import { TagService } from './../../../Shared/services/tag.service';
import { CategoryService } from './../../../Shared/services/category.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Tag } from 'src/app/Shared/models/tag';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, catchError, ignoreElements } from 'rxjs/operators';
import { of } from 'rxjs/index';
@Component({
  selector: 'app-create-news',
  templateUrl: './create-news.component.html',
  styleUrls: ['./create-news.component.css']
})
export class CreateNewsComponent implements OnInit {
  news: SaveNews;
  categories: Category[] = [];
  searchValue;
  markdown = `## Markdown __rulez__!
  ---
  ### Syntax highlight
  \`\`\`typescript
  const language = 'typescript';
  \`\`\`
  ### Lists
  1. Ordered list
  2. Another bullet point
    - Unordered list
    - Another unordered bullet point
  ### Blockquote
  > Blockquote to the max!!!`;

  editMode: boolean;
  sources = [
    this.categoryService.getCategories(),
    this.tagService.getTags()
  ];



  constructor(
    private categoryService: CategoryService,
    private tagService: TagService,
    private newsService: NewsService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) {
    this.news = <SaveNews>{};
    this.news.tags = [];
  }

  ngOnInit() {
    this.categoryService.getCategories()
      .subscribe(res => {
        this.categories = res;
      }, error => console.log(error));

    const newsId = +this.activatedRoute.snapshot.paramMap.get('id');
    if (newsId) {
      // TO DO:
      // forkJoin
      // getting of all the categories
      // getting news for editing
      this.newsService.getNewsById(newsId)
        .subscribe(res => {
          this.news = res;
        }, error => console.log(error));
    }
    this.editMode = (newsId !== 0);
  }
  observableSource = (keyword: any) => {
    if (keyword) {
      return this.tagService.getTagsByPart(keyword)
        .pipe(
          debounceTime(300),
          distinctUntilChanged());
    } else {
      return of(<Tag[]>([]));
    }
  }

  myValueFormatter(data: Tag): string {
    return `(${data.id}) ${data.name}`;
  }
  deleteTag(tagId: number) {
    console.log('tagId->' + tagId);
    const ind = this.news.tags.findIndex(t => t.id === tagId);
    this.news.tags.splice(ind, 1);
  }
  addExistingTag(tag) {
    console.log(JSON.stringify(tag));
    if (this.news.tags.some(t => t.name === this.searchValue.name)) {
      return;
    }
    if (tag) { // for new tag
      this.news.tags.push({ id: 0, name: this.searchValue.name });
      return;
    }
    this.news.tags.push(this.searchValue);
    this.searchValue = null;
  }
}
