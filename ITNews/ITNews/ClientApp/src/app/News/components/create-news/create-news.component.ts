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
import { faFileDownload } from '@fortawesome/free-solid-svg-icons';
import { SaveNews } from '../../models/SaveNews';
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
  photo;
  fileImageDownload = faFileDownload;

  bsEditorInstance: EditorInstance;
  showEditor = true;
  templateForm: FormGroup;
  editorOptions: EditorOption;

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
    this.news.categories = [];
    this.news.markDown = `### Markdown example
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
  }

  ngOnInit() {

    this.editorOptions = {
      autofocus: false,
      iconlibrary: 'fa',
      savable: false,
      onShow: (e) => this.bsEditorInstance = e,
      parser: (val) => this.parse(val)
    };


    this.buildForm(this.news.markDown);


    this.categoryService.getCategories()
      .subscribe(res => {
        this.categories = res;
        console.log(this.categories);
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
  addCustomTag($event) {
    console.log($event);
  }
  addTag() {
    console.log('searchValue -> ' + this.searchValue);

    if (this.searchValue !== '' && !this.news.tags.some(t => t.name === this.searchValue.name)) {
      this.news.tags.push(this.searchValue);
    }
    this.searchValue = null;
  }
  onNewTag($event) {
    if ($event.keyCode === 13 && this.searchValue) {
      console.log($event);
      if (!this.news.tags.some(t => t.name === this.searchValue )) {
        this.news.tags.push({ id: 0, name: this.searchValue });
        this.searchValue = null;
      }
    }
  }
  toggleCategory(id, $event) {
    if ($event.target.checked) {
      const elemToAdd = this.categories.find(c => c.id === id);
      this.news.categories.push(elemToAdd);
    } else {
      const indToRemove = this.news.categories.findIndex(c => c.id === id);
      this.news.categories.splice(indToRemove, 1);
    }
    console.log(JSON.stringify(this.news.categories));
  }

  sendNews() {
    console.log(JSON.stringify(this.news));
    this.newsService.createNews(this.news)
      .subscribe(res => {
        console.log('News has been created');
        console.log(res);
        this.router.navigate(['/']);
      });
  }
    changeUpload() {
      const nativeElement: HTMLInputElement = this.fileInput.nativeElement;
      console.log(nativeElement.files[0].name);
      this.photoName = nativeElement.files[0].name;
      this.photo = nativeElement.files[0];
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
