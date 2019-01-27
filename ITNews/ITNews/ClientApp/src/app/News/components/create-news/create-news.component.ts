import { PhotoService } from './../../../Shared/services/photo-service';
import { Category } from './../../../Shared/models/category';
import { NewsService } from './../../services/news.service';
import { TagService } from './../../../Shared/services/tag.service';
import { CategoryService } from './../../../Shared/services/category.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Tag } from 'src/app/Shared/models/tag';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, catchError, ignoreElements } from 'rxjs/operators';
import { of } from 'rxjs/index';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MarkdownService } from 'ngx-markdown';
import { EditorInstance, EditorOption } from 'angular-markdown-editor/lib/angular-markdown-editor';
@Component({
  selector: 'app-create-news',
  templateUrl: './create-news.component.html',
  styleUrls: ['./create-news.component.css']
})
export class CreateNewsComponent implements OnInit {
  news: SaveNews;
  categories: Category[] = [];
  searchValue;
  @ViewChild('fileInput') fileInput: ElementRef;
  photoName: string;

  bsEditorInstance: EditorInstance;
  markdownText: string;
  showEditor = true;
  templateForm: FormGroup;
  editorOptions: EditorOption;
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
    private router: Router,
    private fb: FormBuilder,
    private markdownService: MarkdownService,
    private photoService: PhotoService
  ) {
    this.news = <SaveNews>{};
    this.news.tags = [];
  }

  ngOnInit() {
    this.editorOptions = {
      autofocus: false,
      iconlibrary: 'fa',
      savable: false,
      onShow: (e) => this.bsEditorInstance = e,
      parser: (val) => this.parse(val)
    };

    // put the text completely on the left to avoid extra white spaces
    this.markdownText =
`### Markdown example
---
This is an **example** where we bind a variable to the \`markdown\` component that is also bind to a textarea.
#### example.component.ts
\`\`\`javascript
function hello() {
  alert('Hello World');
}
\`\`\`
#### example.component.css
\`\`\`css
.bold {
  font-weight: bold;
}
\`\`\``;

    this.buildForm(this.markdownText);


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
  tagSource = (keyword: any) => {
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

  sendNews(form) {
    console.log(JSON.stringify(form));
  }
    changeUpload() {
      const nativeElement: HTMLInputElement = this.fileInput.nativeElement;
      console.log(nativeElement.files[0].name);
      // this.photoService.createNewsPhoto(nativeElement.files[0])
      //   .subscribe(res => console.log(res));
    }

  buildForm(markdownText) {
    this.templateForm = this.fb.group({
      body: [markdownText],
      isPreview: [true]
    });
  }

  /** highlight all code found, needs to be wrapped in timer to work properly */
  highlight() {
    setTimeout(() => {
      this.markdownService.highlight();
    });
  }

  hidePreview() {
    if (this.bsEditorInstance && this.bsEditorInstance.hidePreview) {
      this.bsEditorInstance.hidePreview();
    }
  }

  showFullScreen(isFullScreen: boolean) {
    if (this.bsEditorInstance && this.bsEditorInstance.setFullscreen) {
      this.bsEditorInstance.showPreview();
      this.bsEditorInstance.setFullscreen(isFullScreen);
    }
  }

  parse(inputValue: string) {
    const markedOutput = this.markdownService.compile(inputValue.trim());
    this.highlight();

    return markedOutput;
  }
}
