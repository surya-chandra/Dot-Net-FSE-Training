# Git-HOL 1: Setup & Configuration
git config --global user.name "John Doe"
git config --global user.email "john@example.com"
git config --global core.editor "notepad++ -multiInst -nosession"
git init GitDemo
cd GitDemo
echo "Welcome to Git repository" > welcome.txt
git add welcome.txt
git commit -m "Initial commit with welcome.txt"

# Git-HOL 2: .gitignore setup
echo "*.log" >> .gitignore
echo "log/" >> .gitignore
git add .gitignore
git commit -m "Add .gitignore for log files and log directory"

# Git-HOL 3: Branching & Merging
git branch GitNewBranch
git checkout GitNewBranch
echo "Feature details" > feature.txt
git add feature.txt
git commit -m "Add feature.txt in GitNewBranch"
git checkout master
git merge GitNewBranch
git branch -d GitNewBranch

# Git-HOL 4: Conflict Resolution
git checkout master
echo "<root><message>Master Content</message></root>" > hello.xml
git add hello.xml
git commit -m "Add hello.xml on master"

git checkout -b GitWork
echo "<root><message>Branch Content</message></root>" > hello.xml
git add hello.xml
git commit -m "Update hello.xml on GitWork"

git checkout master
echo "<root><message>Updated Master Content</message></root>" > hello.xml
git add hello.xml
git commit -m "Update hello.xml on master to create conflict"

git merge GitWork
# Resolve conflict in hello.xml
echo "<root><message>Resolved Content</message></root>" > hello.xml
git add hello.xml
git commit -m "Resolved merge conflict on hello.xml"
git branch -d GitWork

# Git-HOL 5: Remote Operations
git remote add origin https://github.com/example/GitDemo.git
git pull origin master --allow-unrelated-histories
# Push changes when ready
# git push origin master
