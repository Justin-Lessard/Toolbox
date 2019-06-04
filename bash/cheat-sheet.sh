# Look for specific text in files matching a pattern
find . -name "*.csproj" -print -exec grep "findtext" {} \;
