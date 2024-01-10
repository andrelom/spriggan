#!/bin/bash

function set_environment_variables {
  file=".env.local"

  if [ ! -f "$file" ]; then
    echo "Could not open $file"
    exit 1
  fi

  echo "Parsed .env.local file"

  while IFS= read -r line; do
    if [[ $line == \#* ]]; then
      continue
    fi

    if [[ -n "$line" ]]; then
      line="${line//\"/}"
      IFS='=' read -r key value <<<"$line"

      if [ -n "$key" ] && [ -n "$value" ]; then
        echo "Setting value $value for key $key"
        export "$key=$value"
      fi
    fi
  done <"$file"
}
